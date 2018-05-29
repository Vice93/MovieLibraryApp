using System.Net.NetworkInformation;

namespace MovieLibrary.ApiSearch
{
    /// <summary>
    /// The connection class
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Gets a value indicating whether this instance is internet connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is internet connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsInternetConnected { get; } = NetworkInterface.GetIsNetworkAvailable();
    }
}
