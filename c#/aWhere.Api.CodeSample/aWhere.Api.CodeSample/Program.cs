using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace aWhere.Api.CodeSample {
    internal class Program {
/* **************************************************************************
 * First let's set up some test variables. You'll need to put your API Key
 * and Secret (also called Consumer Key and Secret) in the first two variables.
 * If you don't have your credentials yet, follow the steps at
 * developer.awhere.com/api/get-started
 * **************************************************************************
 */
#region Constants
        public const string API_KEY = "";
        public const string API_SECRET = "";
        public const String HOST = "https://api.awhere.com";
        public static readonly string OAUTHTOKEN = GetOAuthToken(API_KEY, API_SECRET).Result;
#endregion Constants

#region Methods

/// <summary>
/// Uses the Token API to retrieve an access token.
/// Note: the token will expire after an hour. This API returns an 'expires_in' property
/// with the number of seconds until it expires, but that is not captured in this example.
/// API calls with an expired token also return 401 Unauthorized HTTP error.
/// If there is a problem and the API call can't execute, this function throws an Exception
/// </summary>
/// <param name="api_key">the API Key </param>
/// <param name="api_secret">your specific API Secret</param>
/// <returns></returns>
private async static Task<String> GetOAuthToken(string api_key, string api_secret) {
            String oauthToken = String.Empty;

            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Accept.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                string encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(API_KEY+":"+API_SECRET));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedString);

                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                HttpResponseMessage response = await httpClient.PostAsync(HOST + "/oauth/token", content);

                if (response.IsSuccessStatusCode) {
                    dynamic jsonResponse = await response.Content.ReadAsAsync<ExpandoObject>();
                    oauthToken = (string)jsonResponse.access_token;
                    oauthToken = oauthToken.Trim();
                }
            }

            return oauthToken;
        }

        private static void Main(string[] args) {
            Console.WriteLine("Select a request option");
            Console.WriteLine();
            Console.WriteLine("1. Get All Fields");
            Console.WriteLine("2. Create A Field");
            Console.WriteLine("3. Get Weather Observations");
            Console.WriteLine("4. Get Weather Forecasts");
            Console.WriteLine("5. Get Three Year Weather Norms");
            Console.WriteLine("6. Get Ten Year Weather Norms");
            bool useFahrenheit = false;

            string jsonResponse = string.Empty;

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1) {
                string queryString = Fields.BuildFieldsUrl();
                jsonResponse = MakeAPICall("GET", queryString, null).Result;
            } else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2) {
                string queryString = Fields.BuildFieldsUrl();
                string payload = Fields.BuildCreateFieldPayload();
                jsonResponse = MakeAPICall("POST", queryString, payload).Result;
            } else if (keyInfo.Key == ConsoleKey.D3 || keyInfo.Key == ConsoleKey.NumPad3) {
                string queryString = Weather.BuildObservationsUrl();
                jsonResponse = MakeAPICall("GET", queryString, null).Result;
            } else if (keyInfo.Key == ConsoleKey.D4 || keyInfo.Key == ConsoleKey.NumPad4) {
                string queryString = Weather.BuildForecastsUrl();
                jsonResponse = MakeAPICall("GET", queryString, null).Result;
            } else if (keyInfo.Key == ConsoleKey.D5 || keyInfo.Key == ConsoleKey.NumPad5) {
                string queryString = Weather.BuildThreeYearNormsUrl();
                jsonResponse = MakeAPICall("GET", queryString, null).Result;
            } else if (keyInfo.Key == ConsoleKey.D6 || keyInfo.Key == ConsoleKey.NumPad6) {
                string queryString = Weather.BuildTenYearNormsUrl();
                jsonResponse = MakeAPICall("GET", queryString, null).Result;
            }

            Console.WriteLine(jsonResponse);

            Console.WriteLine();
            Console.WriteLine("Press any key to Exit..+");
            Console.ReadKey();
        }

        /// <summary>
        /// Configures and uses Web API 2.1 client library to open an HTTP transaction with the URL provided
        /// and then converts the response to a PHP object.
        /// If there is a problem and the API call can't execute, this function
        /// throws an Exception using the Web API 2.1 client library
        /// </summary>
        /// <returns>Task resulting in the Json response</returns>
        private static async Task<string> MakeAPICall(string verb, string url, string payload) {
            string responseContent = string.Empty;

            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAUTHTOKEN);
                HttpResponseMessage response;

                switch(verb){
                    case "GET":
                        response = await httpClient.GetAsync(url);
                        break;

                    case "POST":
                        response = await httpClient.PostAsJsonAsync<string>(url, payload);
                        break;

                    default:
                        response = await httpClient.GetAsync(url);
                        break;
                }

                if (response.IsSuccessStatusCode) {
                    responseContent = await response.Content.ReadAsStringAsync();
                }
            }

            return responseContent;
        }

        #endregion Methods
    }
}