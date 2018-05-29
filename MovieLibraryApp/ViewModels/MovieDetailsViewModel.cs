using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    /// <summary>
    /// The movie details viewmodel.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class MovieDetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// The lookup search class.
        /// </summary>
        private readonly LookupSearch _ls = new LookupSearch();

        /// <summary>
        /// Gets or sets the movie object.
        /// </summary>
        /// <value>
        /// The movie object.
        /// </value>
        public Movie MovieObject { get; set; }

        /// <summary>
        /// Lookups a movie with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ObservableCollection<Movie>> Lookup(string id)
        {
            if (new Connection().IsInternetConnected) return await _ls.GetMovieInfoAsync(id);
            return new ObservableCollection<Movie>();
        }
    }
}
