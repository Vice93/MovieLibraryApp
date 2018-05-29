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
                        cmd.CommandText = "select MovieId from dbo.UserHasMovie where UserId=@UserId";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var mov = new Movie()
                                {
                                    MovieId = reader.GetString(0)
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
                const string filePath = @"C:\Error.txt";
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
        /// <returns></returns>
        public bool InsertFavoriteMovieIntoDb(string userId,string movieId)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "insert into dbo.UserHasMovie (Userid,MovieId) values (@UserId, @MovieId)";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieID", movieId);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @".\Error.txt";
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
                        cmd.CommandText = "delete from dbo.UserHasMovie where UserId=@UserId and MovieId=@MovieId";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @".\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + e.Message + "<br/>" + Environment.NewLine + "StackTrace :" + e.StackTrace +
                                     "" + Environment.NewLine + "Date :" + DateTime.Now);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return false;
            }
        }

        public bool UpdateFavoriteMovieInDb(string userId, string oldMovieId, string newMovieId)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "update dbo.UserHasMovie set dbo.UserHasMovie.MovieId=@NewMovieId where UserId=@UserId and MovieId=@MovieId";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@MovieId", oldMovieId);
                        cmd.Parameters.AddWithValue("@NewMovieId", newMovieId);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                const string filePath = @".\Error.txt";
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
