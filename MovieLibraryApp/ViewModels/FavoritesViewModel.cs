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
                    var res = await client.GetStringAsync(_baseUri + "/favorites?userId=" + User.UserId); 
                    var movies = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(res);

                    for (var i = 0; i < movies.Count; i++)
                    {
                        if (!User.FavoriteMoviesIds.Contains(movies[i].MovieId)) User.FavoriteMoviesIds.Add(movies[i].MovieId);
                    }

                    return movies;
                }
            }
            catch (HttpRequestException e)
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
            catch(HttpRequestException e)
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
        public async Task<bool> InsertMovieInDb(string movieId, string movieTitle, string imgRef)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new InsertModel
                    {
                        UserId = User.UserId.ToString(),
                        MovieId = movieId,
                        MovieTitle = movieTitle,
                        ImgRef = imgRef
                    };

                    var res = await client.PostAsync(_baseUri + "/favorites", SerializeJsonContent(model));

                    return res.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException e)
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
                        NewMovie = "mhmov3jmoaTtkI4mfmD0vxbGPAbt6bggCUchYZRG4Om9", //Sample movie for PUT request(Lord of the Rings: The return of the King). It's a great movie so I'm sure the user don't mind.
                        MovieTitle = "The Lord of the Rings: The Return of the King",
                        ImgRef = "https://images.mediahound.com/media/mhimgEJ2fdWGaD4D1WqjikaSsR6tVCWM7TDd6wcDorjg.jpg"
                    };

                    var res = await client.PutAsync(_baseUri + "/favorites", SerializeJsonContent(model));

                    return res.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException e)
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
        private class InsertModel
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
            /// <summary>
            /// Gets or sets the movie title.
            /// </summary>
            /// <value>
            /// The movie title.
            /// </value>
            public string MovieTitle { get; set; }
            /// <summary>
            /// Gets or sets the img reference.
            /// </summary>
            /// <value>
            /// The img reference.
            /// </value>
            public string ImgRef { get; set; }
        }
    }
}
