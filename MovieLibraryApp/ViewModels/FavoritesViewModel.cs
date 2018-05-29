using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    /// <summary>
    /// The favorites viewmodel
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class FavoritesViewModel : ViewModelBase
    {
        /// <summary>
        /// The base URI
        /// </summary>
        private readonly Uri _baseUri = new Uri("http://localhost:50226/api");

        /// <summary>
        /// Gets or sets the movie object.
        /// </summary>
        /// <value>
        /// The movie object.
        /// </value>
        public Movie MovieObject { get; set; }

        /// <summary>
        /// Goes to movie details page.
        /// </summary>
        public void GoToMovieDetailsPage()
        {
            if (MovieObject == null) return;
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
        }

        /// <summary>
        /// Gets the favorite movies.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Movie>> GetFavoriteMovies()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    LookupSearch ls = new LookupSearch();

                    var res = await client.GetStringAsync(_baseUri + "/favorites?userId=" + User.UserId); 
                    var movies = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(res);

                    var idString = "";
                    for (var i = 0; i < movies.Count; i++)  // This correctly adds as many ids as you have in the database to the search string, however mediahound didn't add a "next" page
                    {                                       // to the results from a lookup on more than 10 movies. Guess they didn't anticipate that anyone would need to look up more than 10 movies at once?
                        if (!User.FavoriteMoviesIds.Contains(movies[i].MovieId)) User.FavoriteMoviesIds.Add(movies[i].MovieId);

                        idString += movies[i].MovieId;
                        if (i != movies.Count - 1) idString += "\",\"";
                    }

                    var result = await ls.GetMovieInfoAsync(idString);

                    if (result.Count != 0) return result;

                    User.FavoriteMoviesIds.Clear();
                    return new ObservableCollection<Movie>();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new ObservableCollection<Movie>();
            }
        }

        /// <summary>
        /// Deletes the movie from database.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteMovieFromDb(string movieId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var res = await client.DeleteAsync(_baseUri + "/favorites?userId=" + User.UserId + "&movieId=" + movieId);

                    return res.IsSuccessStatusCode;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts the movie in database.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<bool> InsertMovieInDb(string movieId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new InsertModel
                    {
                        UserId = User.UserId.ToString(),
                        MovieId = movieId
                    };

                    var res = await client.PostAsync(_baseUri + "/favorites", SerializeJsonContent(model));

                    return res.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Puts the movie in database.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<bool> PutMovieInDb(string movieId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new InsertModel
                    {
                        UserId = User.UserId.ToString(),
                        MovieId = movieId,
                        NewMovie = "mhmov3jmoaTtkI4mfmD0vxbGPAbt6bggCUchYZRG4Om9" //Sample movie (Lord of the Rings)
                    };

                    var res = await client.PutAsync(_baseUri + "/favorites", SerializeJsonContent(model));

                    return res.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Serializes the model into a json format and return it as stringcontent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private static StringContent SerializeJsonContent(InsertModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// The model used for PostAsync
        /// </summary>
        public class InsertModel
        {
            /// <summary>
            /// Gets or sets the user identifier.
            /// </summary>
            /// <value>
            /// The user identifier.
            /// </value>
            public string UserId { get; set; }
            /// <summary>
            /// Gets or sets the movie identifier.
            /// </summary>
            /// <value>
            /// The movie identifier.
            /// </value>
            public string MovieId { get; set; }
            /// <summary>
            /// Gets or sets the new movie. There is no practical application for this, so it's only there because of the requirement to have a Put operation.
            /// </summary>
            /// <value>
            /// The new movie's id.
            /// </value>
            public string NewMovie { get; set; }
        }
    }
}
