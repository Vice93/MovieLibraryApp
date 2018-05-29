using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using MovieLibrary.Models.Model;

namespace MovieLibrary.DbAccess
{
    /// <summary>
    /// The database connection class
    /// </summary>
    public class DbConnection
    {
        /// <summary>
        /// The database connection string. Much safe, such wow.
        /// </summary>
        private const string ConnectionString = @"Data Source=donau.hiof.no;Initial Catalog=jonasv;Integrated Security=False;User ID=jonasv;Password=Sp58y2;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Gets the users favorite movies from database.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Collection<Movie> GetFavoriteMoviesFromDb(string userId)
        {
            Collection<Movie> movieList = new Collection<Movie>();
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return null;
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select MovieId,MovieTitle,ImageRef from dbo.UserHasMovie where UserId=@UserId;";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var mov = new Movie()
                                {
                                    MovieId = reader.GetString(0),
                                    MovieName = reader.GetString(1),
                                    ImageReference = reader.GetString(2)
                                };
                                movieList.Add(mov);
                            }
                            return movieList;
                        }
                    }
                }
            }
            catch(SqlException e)
            {
                const string filePath = @"C:\logs\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + e.Message + "<br/>" + Environment.NewLine + "StackTrace :" + e.StackTrace +
                                     "" + Environment.NewLine + "Date :" + DateTime.Now);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return null;
            }
        }

        /// <summary>
        /// Inserts the users favorite movie into database.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="movieTitle">The title of the movie</param>
        /// <param name="imgRef">The image reference of the movie</param>
        /// <returns></returns>
        public bool InsertFavoriteMovieIntoDb(string userId,string movieId,string movieTitle, string imgRef)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "insert into dbo.UserHasMovie (UserId,MovieId,MovieTitle,ImageRef) values (@UserId, @MovieId, @MovieTitle, @ImageRef);";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        cmd.Parameters.AddWithValue("@MovieTitle", movieTitle);
                        cmd.Parameters.AddWithValue("@ImageRef", imgRef);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @"C:\logs\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + e.Message + "<br/>" + Environment.NewLine + "StackTrace :" + e.StackTrace +
                                     "" + Environment.NewLine + "Date :" + DateTime.Now);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return false;
            }
        }

        /// <summary>
        /// Deletes the users favorite movie from database.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public bool DeleteFavoriteMovieFromDb(string userId,string movieId)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "delete from dbo.UserHasMovie where UserId=@UserId and MovieId=@MovieId;";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @"C:\logs\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + e.Message + "<br/>" + Environment.NewLine + "StackTrace :" + e.StackTrace +
                                     "" + Environment.NewLine + "Date :" + DateTime.Now);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return false;
            }
        }

        /// <summary>
        /// Updates a favorite movie in the database.
        /// Currently it only changes it into Lord of the Rings: The return of the King
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="oldMovieId">The old movie identifier.</param>
        /// <param name="newMovieId">The new movie identifier.</param>
        /// <param name="movieTitle">The title of the movie.</param>
        /// <param name="imgRef">The img reference of the movie.</param>
        /// <returns></returns>
        public bool UpdateFavoriteMovieInDb(string userId, string oldMovieId, string newMovieId, string movieTitle, string imgRef)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "update dbo.UserHasMovie set dbo.UserHasMovie.MovieId=@NewMovieId, dbo.UserHasMovie.MovieTitle=@MovieTitle, dbo.UserHasMovie.ImageRef=@ImageRef where UserId=@UserId and MovieId=@MovieId;";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieId", oldMovieId);
                        cmd.Parameters.AddWithValue("@NewMovieId", newMovieId);
                        cmd.Parameters.AddWithValue("@MovieTitle", movieTitle);
                        cmd.Parameters.AddWithValue("@ImageRef", imgRef);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @"C:\logs\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + e.Message + "<br/>" + Environment.NewLine + "StackTrace :" + e.StackTrace +
                                     "" + Environment.NewLine + "Date :" + DateTime.Now);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return false;
            }
        }
    }
}
