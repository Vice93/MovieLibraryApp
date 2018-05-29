using System;
using System.IO;
using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;
using MovieLibrary.ApiSearch;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The Main page of the application
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MainPage : Page
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieLibraryApp.Views.MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            //Retrieve the OAuth2 token when the app launches
            if (OAuth2.Token == null) Task.Run(GenerateToken); 

            //Get user GUID
            MovieLibrary.Models.Model.User.UserId = new EasClientDeviceInformation().Id;
        }

        /// <summary>
        /// Handles the OnTapped event of the SearchIcon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private void SearchIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Handles the OnKeyUp event of the SearchInput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void SearchInput_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) Search();
        }

        /// <summary>
        /// Perform a search on the user input.
        /// </summary>
        private async void Search()
        {
            if (SearchInput.Text == "") return;
            try
            {
                var mpvm = new MainPageViewModel();
                LoadingIndicator.IsActive = true;
                if (!new Connection().IsInternetConnected) { EmptyList.Text = "You need a working internet connection to perform any search."; return; }

                MainGrid.ItemsSource = await mpvm.NormalSearch(SearchInput.Text);
                EmptyList.Text = MainGrid.Items.Any() ? "" : "'" + SearchInput.Text + "' didn't return any results. Try searching for something else!";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        /// <summary>
        /// Generates the OAuth2token.
        /// </summary>
        /// <returns></returns>
        private static async Task GenerateToken()
        {
            await OAuth2.GenerateAuth2TokenAsync("Retrieved OAuth2 token on app launch.");
        }
    }
}