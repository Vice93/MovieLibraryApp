using System;

namespace MovieLibrary.ApiSearch
{
    /// <summary>
    /// The DateTime conversion class.
    /// </summary>
    public class ConvertDateTime
    {
        //Copied from stackoverflow: https://stackoverflow.com/a/3354915/5309584
        /// <summary>
        /// Converts from unix timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// Converts to unix timestamp.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
