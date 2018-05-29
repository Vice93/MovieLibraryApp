using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    /// <summary>
    /// The viewmodel for the ExploreSearch page
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class ExploreSearchViewModel : ViewModelBase
    {
        /// <summary>
        /// The ExploreSearch class
        /// </summary>
        private readonly ExploreSearch _explore = new ExploreSearch();

        /// <summary>
        /// Gets or sets the movie object.
        /// </summary>
        /// <value>
        /// The movie object.
        /// </value>
        public Movie MovieObject { get; set; }

        /// <summary>
        /// Goes to movie details page when clicking a movie.
        /// </summary>
        public void GoToMovieDetailsPage()
        {
            if (MovieObject == null) return;
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
        }

        /// <summary>
        /// Looks for movies based on the params.
        /// </summary>
        /// <param name="genre">The genre.</param>
        /// <param name="year">The year.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public async Task<ObservableCollection<Movie>>Explore(string genre, string year, string type)
        {
            if (new Connection().IsInternetConnected) return await _explore.ExploreMovies(genre, year, type);
            return new ObservableCollection<Movie>();
        }
    }
}
