using Template10.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using MovieLibrary.ApiSearch;

namespace MovieLibraryApp.ViewModels
{
    /// <summary>
    /// The main page viewmodel.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The search class.
        /// </summary>
        private readonly Search _search = new Search();
        /// <summary>
        /// Gets or sets the designtime sample list.
        /// </summary>
        /// <value>
        /// The designtime samplelist.
        /// </value>
        public ObservableCollection<Movie> DesignSample { get; set; }

        /// <summary>
        /// Gets or sets the movie object.
        /// </summary>
        /// <value>
        /// The movie object.
        /// </value>
        public Movie MovieObject { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                DesignSample = new ObservableCollection<Movie>()
                {
                    new Movie
                    {
                        MovieName = "SampleMovie 1",
                        ImageReference = "/Assets/banner.jpg",
                    },
                    new Movie
                    {
                        MovieName = "SampleMovie 2",
                        ImageReference = "/Assets/banner.jpg",
                    },
                    new Movie
                    {
                        MovieName = "SampleMovie 3",
                        ImageReference = "/Assets/banner.jpg",
                    },
                    new Movie
                    {
                        MovieName = "SampleMovie 4",
                        ImageReference = "/Assets/banner.jpg",
                    }
                };
            }
        }

        /// <summary>
        /// Gotoes the details page.
        /// </summary>
        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage));

        /// <summary>
        /// Gotoes the settings.
        /// </summary>
        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        /// <summary>
        /// Gotoes the privacy.
        /// </summary>
        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        /// <summary>
        /// Gotoes the about.
        /// </summary>
        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        /// <summary>
        /// Goes to movie details page.
        /// </summary>
        public void GoToMovieDetailsPage()
        {
            if (MovieObject == null) return;
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
        }

        /// <summary>
        /// Performs a search in the search class.
        /// </summary>
        /// <param name="searchInput">The search input.</param>
        /// <returns></returns>
        public async Task<ObservableCollection<Movie>> NormalSearch(string searchInput)
        {
            return await _search.SearchForMovie(searchInput);
        }
    }
}
