using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibrary.ApiSearch
{
    /// <summary>
    /// The search class
    /// </summary>
    public class Search
    {
        /// <summary>
        /// The movie list
        /// </summary>
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        /// <summary>
        /// Searches the mediahound /search endpoint for movies based on the search input.
        /// </summary>
        /// <param name="searchInput">The search input.</param>
        /// <returns name="_movieList">The ObservableCollection containing Movie objects</returns>
        public async Task<ObservableCollection<Movie>> SearchForMovie(string searchInput)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/search/all/");
            var param = searchInput + "?types=movie&types=showseries";
            _movieList.Clear();

            var response = await Client.ClientRequest(HttpMethod.Get, baseUri + param);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await OAuth2.GenerateAuth2TokenAsync("OAuth2 token expired. Generated a new one.");
                response = await Client.ClientRequest(HttpMethod.Get, baseUri + param);
            }
            if (!response.IsSuccessStatusCode) return _movieList;

            var content = await response.Content.ReadAsStringAsync();

            JObject jobject = JObject.Parse(content);
            JToken movies = jobject["content"];

            if (movies.Any()) await AddMoviesToList(movies);

            await NextResults(jobject);

            return _movieList;
        }
        
        
        /// <summary>
        /// Gets the next page URI and performs a new search if it exists.
        /// Because of API limiations, it performs this search 4 extra times for a total of 50 results, however you can increase it to as many as you'd like.
        /// Just keep in mind there's a limit of 200 calls per day.
        /// </summary>
        /// <param name="jobject">The jobject.</param>
        /// <returns></returns>
        private async Task NextResults(JObject jobject)
        {
            for (var i = 0; i < 4; i++)
            {
                var next = jobject["pagingInfo"]["next"].ToString();
                if (!next.Any()) return;

                var result = await Client.ClientRequest(HttpMethod.Get, next);
                var content = await result.Content.ReadAsStringAsync();

                jobject = JObject.Parse(content);
                var movies = jobject["content"];

                await AddMoviesToList(movies);
            }
        }

        /// <summary>
        /// Adds the movies to the Observable Collection.
        /// </summary>
        /// <param name="movies">The movies.</param>
        /// <returns></returns>
        private async Task AddMoviesToList(JToken movies)
        {
            for (var i = 0; i < movies.Count(); i++)
            {
                try
                {
                    var movie = movies[i];

                    var imgRef = (string)movie["object"]["primaryImage"]["object"]["small"]["url"] ?? "/Assets/noImageAvailable.png";
                    var mov = new Movie
                    {
                        MovieId = (string)movie["object"]["mhid"],
                        MovieName = (string)movie["object"]["name"],
                        ImageReference = imgRef
                    };
                    _movieList.Add(mov);
                }
                catch (NullReferenceException e)
                {
                    Debug.WriteLine(e.Message);
                    break;
                }
            }
            await Task.CompletedTask;
        }
    }
}
