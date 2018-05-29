using System;
using System.Collections.Generic;

namespace MovieLibrary.Models.Model
{
    /// <summary>
    /// The user object class
    /// </summary>
    public static class User
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public static Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the favorite movies ids.
        /// </summary>
        /// <value>
        /// The favorite movies ids.
        /// </value>
        public static List<string> FavoriteMoviesIds { get; set; } = new List<string>();
    }
}
