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
    /// The explore search class
    /// </summary>
    public class ExploreSearch
    {
        /// <summary>
        /// The movie list
        /// </summary>
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        /// <summary>
        /// Searches the mediahound /explore endpoint for movies based the parameters.
        /// </summary>
        /// <param name="genre">The genre.</param>
        /// <param name="year">The year.</param>
        /// <param name="type">The type.</param>
        /// <returns>The observable collection containing movie objects.</returns>
        public async Task<ObservableCollection<Movie>> ExploreMovies(string genre, string year, string type)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/explore?params=");

            _movieList.Clear();

            var typeWithoutDash = type.Replace("/", string.Empty);
            var param = Uri.EscapeUriString("{\"filters\": {\"returnType\": {\"$eq\": \"" + typeWithoutDash + "\"},\"traits\": {\"$eq\": \"mhgnr-" + genre.ToLower() + "\"},\"year\": {\"$in\":[" + year + "]}}, \"components\":[\"primaryImage\"]}");
                
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
        /// </summary>
        /// <param name="jobject">The jobject.</param>
        /// <returns></returns>
        private async Task NextResults(JObject jobject)
        {
            for (var i = 0; i < 4; i++) //Because I have a very limited amount of requests to make (Student version, not paid), I only make the request 4 times (5 in total for 50 results). Limit is 200 calls per day (2000 results).
            {
                var next = jobject["pagingInfo"]["next"].ToString(); //Somehow the response from mediahound is nesting the components part of the 'next' url causing it to fail because its nested >3 times, I have no clue why because it doesn't do it in /search.
                if (!next.Any()) return;
                
                int index = next.IndexOf("%2C%22components%22%3A%5B%7B%22name%22%3A%22metadata%22"); //Remove metadata components from the string so we can get the next pages.
                if (index >= 0) next = next.Remove(index) + "}]}";
                
                var result = await Client.ClientRequest(HttpMethod.Get, next);

                if (!result.IsSuccessStatusCode) return;
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
        public async Task AddMoviesToList(JToken movies)
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
                    /* 'Cuz I'm one step closer to the edge,*/
                    /* and I'm about to */ break;
                }
            }
            await Task.CompletedTask;
        }
    }
}
