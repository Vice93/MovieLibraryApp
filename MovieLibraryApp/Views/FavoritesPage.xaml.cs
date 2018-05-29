using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using MovieLibrary.ApiSearch;
using Template10.Services.SerializationService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The favorites page
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class FavoritesPage : Page
    {
        /// <summary>
        /// The ViewModel for this page
        /// </summary>
        private readonly FavoritesViewModel _fvm;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieLibraryApp.Views.FavoritesPage" /> class.
        /// </summary>
        public FavoritesPage()
        {
            InitializeComponent();
            _fvm = new FavoritesViewModel();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                if (!new Connection().IsInternetConnected)
                {
                    NoConnection.Text = "You need a working internet connection to see your favorite movies.";
                    return;
                }
                NoConnection.Text = "";

                LoadingIndicator.IsActive = true;

                var res = await _fvm.GetFavoriteMovies();

                if (res != null) MainGrid.ItemsSource = res;
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        /// <summary>
        /// Handles the OnTapped event of the PutRequestButton control. 
        /// This button is purely here for showcasing a PUT request, however there is not a practical need for this functionality yet. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private async void PutRequestButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (!new Connection().IsInternetConnected)
                {
                    NoConnection.Text = "You need a working internet connection to update this movie.";
                    return;
                }

                NoConnection.Text = "";

                LoadingIndicator.IsActive = true;

                var buttonParent = (StackPanel)((Button)sender).Parent;

                await _fvm.PutMovieInDb(buttonParent.Tag.ToString());

                var res = await _fvm.GetFavoriteMovies();

                if (res != null) MainGrid.ItemsSource = res;
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }
    }
}
