using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieLibrary.DbAccess;
using MovieLibrary.Models.Model;
using Newtonsoft.Json;

namespace MovieLibrary.Api.Controllers
{
    /// <summary>
    /// The API controller for favorite movies
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class FavoritesController : ApiController
    {
        /// <summary>
        /// The database connection
        /// </summary>
        private readonly DbConnection _dbConnection = new DbConnection();

        // GET: api/favorites
        /// <summary>
        /// Gets all movies for user with userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/favorites")]
        public IEnumerable<Movie> Get([FromUri]string userId)
        {
            var res = _dbConnection.GetFavoriteMoviesFromDb(userId);
            
            return res;

            //These could use much more work to check for everything
        }

        // POST: api/Favorites
        /// <summary>
        /// Insert a user and its favorite movie into the database.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/favorites")]
        public HttpResponseMessage Post()
        {
            var requestContent = Request.Content;
            var jsonContent = requestContent.ReadAsStringAsync().Result;
            var insert = JsonConvert.DeserializeObject<InsertModel>(jsonContent);

            if (_dbConnection.InsertFavoriteMovieIntoDb(insert.UserId,insert.MovieId)) return Request.CreateResponse(HttpStatusCode.OK, "Saved movie");
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Couldn't post movie.");

            //These could use much more work to check for everything
        }

        // PUT: api/Favorites/5
        /// <summary>
        /// Update a users movie.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/favorites")]
        public HttpResponseMessage Put()
        {
            var requestContent = Request.Content;
            var jsonContent = requestContent.ReadAsStringAsync().Result;
            var insert = JsonConvert.DeserializeObject<InsertModel>(jsonContent);

            if (_dbConnection.UpdateFavoriteMovieInDb(insert.UserId, insert.MovieId, insert.NewMovie)) return Request.CreateResponse(HttpStatusCode.OK, "Updated movie.");
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Couldn't update movie.");
        }

        // DELETE: api/Favorites/5
        /// <summary>
        /// Deletes the movie belonging to user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/favorites")]
        public HttpResponseMessage Delete([FromUri]string userId, string movieId)
        {
            if(_dbConnection.DeleteFavoriteMovieFromDb(userId,movieId)) return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Couldn't delete movie.");

            //These could use much more work to check for everything
        }
    }

    /// <summary>
    /// The model used to read the Post/Put content.
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
        /// Gets or sets the new movie's id.
        /// </summary>
        /// <value>
        /// The new movie identifier to replace an old with.
        /// </value>
        public string NewMovie { get; set; }
    }
}
