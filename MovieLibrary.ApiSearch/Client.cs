using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MovieLibrary.ApiSearch
{
    /// <summary>
    /// The HttpClient class
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// Sends a request to the specified uri
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="uri">The uri string.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> ClientRequest(HttpMethod method, string uri)
        {
            using (var client = new HttpClient())
            {
                if (OAuth2.Token == null) await OAuth2.GenerateAuth2TokenAsync("Token was null. Retrieved new one."); //Should never get invoked.

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(OAuth2.TokenType, OAuth2.Token);
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(method, uri);

                return await client.SendAsync(request);
            }
        }
    }
}
