using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using Template10.Services.SerializationService;
using MovieLibrary.Models.Model;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using MovieLibrary.ApiSearch;
using Windows.UI.Notifications;

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The movie details page
    /// </summary>
    public sealed partial class MovieDetailsPage
    {
        /// <summary>
        /// The Moviedetails viewmodel
        /// </summary>
        private readonly MovieDetailsViewModel _mdvm;
        /// <summary>
        /// The template10 serialization service
        /// </summary>
        private readonly ISerializationService _serializationService;
        /// <summary>
        /// The movie identifier
        /// </summary>
        private string _movieId = "";
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieLibraryApp.Views.MovieDetailsPage" /> class.
        /// </summary>
        public MovieDetailsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            _mdvm = new MovieDetailsViewModel();
            _serializationService = SerializationService.Json;
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null) return;

            if (!new Connection().IsInternetConnected)
            {
                AddToFavorites.Visibility = Visibility.Collapsed;
                ShowToastNotification("No internet", "You need an internet connection to view this movie's details.");
                return;
            }

            try
            {
                LoadingIndicator.IsActive = true;
                AddToFavorites.Visibility = Visibility.Collapsed;

                _movieId = _serializationService.Deserialize(e.Parameter?.ToString()).ToString();
                var res = await _mdvm.Lookup(_movieId);
                MainGrid.ItemsSource = res;

                CheckIfMovieIsFavorite(_movieId);
            }
            finally
            {
                LoadingIndicator.IsActive = false;
                AddToFavorites.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the Tapped event of the AddToFavorites control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private async void AddToFavorites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FavoritesViewModel fvm = new FavoritesViewModel();

            if (!new Connection().IsInternetConnected) ShowToastNotification("No internet", "You need an internet connection to add or remove a movie from favorites.");

            if (CheckIfMovieIsFavorite(_movieId)) // Delete from db
            {
                var response = await fvm.DeleteMovieFromDb(_movieId);

                if (response) User.FavoriteMoviesIds.Remove(_movieId);
            }
            else //Add to db
            {
                var response = await fvm.InsertMovieInDb(_movieId);

                if (response) User.FavoriteMoviesIds.Add(_movieId);
            }
            CheckIfMovieIsFavorite(_movieId);
        }

        /// <summary>
        /// Checks if movie is already a favorite, and change button appearance to match.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool CheckIfMovieIsFavorite(string id)
        {
            if (User.FavoriteMoviesIds.Contains(id)){
                AddToFavorites.Background = new SolidColorBrush(Colors.Red);
                AddToFavorites.Content = "Remove from favorites";
                return true;
            }

            AddToFavorites.Background = new SolidColorBrush(Colors.DarkGray);
            AddToFavorites.Content = "Add to favorites";
            return false;
        }

        // Copied from https://stackoverflow.com/a/37542911/5309584
        /// <summary>
        /// Shows a toast notification. Simply wanted to try out a windows notification somewhere as opposed to displaying a message inside the app.
        /// </summary>
        /// <param name="title">Title of the toast.</param>
        /// <param name="stringContent">Content of the toast.</param>
        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }
    }
}
