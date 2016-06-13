using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using aWhere.Api.ConsoleDemo;

namespace aWhere.Api.Services
{
    public class aWhereApiConnection {
        #region Constructors

        public aWhereApiConnection(string api_key, string api_secret) {
            Menus.PrintEachLetterToConsole("First, let's check for your API Key and Secret.....");
            OAUTHTOKEN = GetOAuthToken(api_key, api_secret).Result;
        }

        #endregion Constructors

        #region Constants

        public const String HOST = "https://api.awhere.com";
        public readonly string OAUTHTOKEN = "";

        #endregion Constants

        #region Properties

        public bool IsAuthenticated { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Uses the Token API to retrieve an access token.
        /// Request Type: POST
        /// Request API Endpoint: https://api.awhere.com/oauth/token
        /// Include the following HTTP Headers with your request:
        ///
        /// Content-Type: application/x-www-form-urlencoded
        /// Authorization: Basic {hashed_credential}
        /// You will need to replace {hashed_credential} with the Base64-encoded {key}:{secret} combination, separated by a colon.
        ///
        /// Note: The token will expire after an hour. This API returns an 'expires_in' property
        /// with the number of seconds until it expires, but that is not captured in this example.
        /// API calls with an expired token also return 401 Unauthorized HTTP error.
        /// If there is a problem and the API call can't execute, this function throws an Exception
        /// </summary>
        /// <param name="api_key">the API Key </param>
        /// <param name="api_secret">your specific API Secret</param>
        /// <returns></returns>
        private async Task<String> GetOAuthToken(string api_key, string api_secret) {
            if (String.IsNullOrEmpty(api_key) || String.IsNullOrEmpty(api_secret)) {
                throw new ArgumentNullException("API Key or Secret is missing. Please check your config settings in the Program.cs file.\n");
            } else {
                Menus.PrintEachLetterToConsole("Found your API Key and Secret! Now, let's obtain an access token.");
            }

            String oauthToken = String.Empty;

            using (var httpClient = new HttpClient()) {
                Menus.PrintEachLetterToConsole("Building HTTP Headers.....");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                // You need to include the following HTTP Headers with your POST request
                // Add the first header: Content-Type: application/x-www-form-urlencoded
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // For the Authorization Header, you need to Base-64 encode your key and secret
                string encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(api_key + ":" + api_secret));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedString);

                // Display the complete HTTP Headers to the Console
                Menus.DisplayHttpHeaders(httpClient.DefaultRequestHeaders.ToString());

                // Request Body: Lastly, be sure to include the following text in the request body: grant_type=client_credentials
                Menus.PrintEachLetterToConsole("Setting up the HTTP Request Body.....");
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                Menus.DisplayHttpBody("grant_type=client_credentials");

                Menus.DisplayCompleteRequest(HOST,
                                    httpClient.DefaultRequestHeaders.Authorization.ToString(),
                                    httpClient.DefaultRequestHeaders.Accept.ToString(), "POST", "grant_type=client_credentials");

                Menus.PrintEachLetterToConsole(String.Format("Attempting to send your POST Request Payload to {0}...... ", HOST + "/oauth/token"));

                HttpResponseMessage response = await httpClient.PostAsync(HOST + "/oauth/token", content);

                if (response.IsSuccessStatusCode) {
                    dynamic jsonResponse = await response.Content.ReadAsAsync<ExpandoObject>();
                    oauthToken = (string)jsonResponse.access_token;
                    oauthToken = oauthToken.Trim();
                    IsAuthenticated = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Menus.WriteLineCentered(String.Format("SUCCESS {0} - {1}\n", (int)response.StatusCode, response.ReasonPhrase));
                    Console.ResetColor();
                    Menus.PrintEachLetterToConsole(String.Format("OAuth Token Obtained! Your token is {0} and will expire after 1 hour.\n", oauthToken));
                } else {
                    IsAuthenticated = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Menus.WriteLineCentered(String.Format("ERROR {0} - {1}\n", (int)response.StatusCode, response.ReasonPhrase));
                    Console.ResetColor();
                    throw new HttpRequestException(String.Format("\nAn error was returned from the server: {0}.\nFor more information about your HTTP Status Code, visit {1}.",
                        response.ReasonPhrase, "http://developer.awhere.com/api/conventions"));
                }
            }

            return oauthToken;
        }

        #endregion Methods
    }
}