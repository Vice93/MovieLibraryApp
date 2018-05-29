using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.ApiSearch
{
    /// <summary>
    /// The OAuth2 class responsible for creating a new token if the old one has expired, is wrong or doesn't exist yet.
    /// </summary>
    public static class OAuth2
    {
        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public static string TokenType { get; set; }
        /// <summary>
        /// Gets or sets token expiration.
        /// </summary>
        /// <value>
        /// Number of milliseconds until the token expires.
        /// </value>
        public static double ExpiresIn { get; set; }
        /// <summary>
        /// Gets or sets the scope of the token.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public static string Scope { get; set; }
        /// <summary>
        /// Gets the OAuth2 token.
        /// </summary>
        /// <value>
        /// The OAuth2 token.
        /// </value>
        public static string Token { get; private set; }

        /// <summary>
        /// Generates the OAuth2 token asynchronous.
        /// </summary>
        /// <param name="action">The action that initiated the method.</param>
        /// <returns></returns>
        public static async Task GenerateAuth2TokenAsync(string action)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var byteArray = Encoding.ASCII.GetBytes("mhclt_dotNetMovieLibrary:TJUPmRCiyPTepw4QxjFT3ND8MxEID2TX23H7yYFegqdWM33Q");
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;
                    client.DefaultRequestHeaders
                          .Accept
                          .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    const string url = "https://api.mediahound.com/1.3/security/oauth/token";
                    var parameters = new Dictionary<string, string> { { "client_id", "mhclt_dotNetMovieLibrary" }, { "client_secret", "TJUPmRCiyPTepw4QxjFT3ND8MxEID2TX23H7yYFegqdWM33Q" }, { "grant_type", "client_credentials" } };
                    var encodedContent = new FormUrlEncodedContent(parameters);

                    var response = await client.PostAsync(url, encodedContent).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        JObject res = JObject.Parse(responseContent);

                        Token = (string)res["access_token"];
                        TokenType = (string)res["token_type"];
                        ExpiresIn = (double)res["expires_in"];
                        Scope = (string)res["scope"];

                        Debug.WriteLine(action);
                    }
                    else
                    {
                        Debug.WriteLine("Couldn't get OAuth2 token. Statuscode: " + response.StatusCode + ", Action: " + action);
                    }
                }
                catch (ArgumentException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
