using System;

namespace MovieLibrary.Models.Model
{
    /// <summary>
    /// The Movie object class
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>
        /// The movie identifier.
        /// </value>
        public string MovieId { get; set; }
        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        /// <value>
        /// The name of the movie.
        /// </value>
        public string MovieName { get; set; }
        /// <summary>
        /// Gets or sets the release date.
        /// </summary>
        /// <value>
        /// The release date.
        /// </value>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the image reference.
        /// </summary>
        /// <value>
        /// The image reference.
        /// </value>
        public string ImageReference { get; set; }
        /// <summary>
        /// Gets or sets the pg-rating.
        /// </summary>
        /// <value>
        /// The pg.
        /// </value>
        public string Pg { get; set; }
        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>
        /// The genre.
        /// </value>
        public string Genre { get; set; }
    }
}
