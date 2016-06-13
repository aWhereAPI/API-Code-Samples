using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using aWhere.Api.ConsoleDemo;
using Newtonsoft.Json;

namespace aWhere.Api.Services {
    public class ApiRequest<T> {
        #region Properties

        public aWhereApiConnection Connection { get; set; }

        #endregion Properties

        #region Constructors

        public ApiRequest(aWhereApiConnection connection) {
            Connection = connection;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Configures and uses Web API 2.1 client library to open an HTTP transaction with the URL provided.
        /// If there is a problem and the API call can't execute, this function
        /// throws an Exception using the Web API 2.1 client library
        /// </summary>
        /// <returns>Task resulting in the Json response</returns>
        public async Task<T> MakeApiCall(string verb, string uri, T postBody) {
            T model;
            string body = "None";

            Menus.DisplayInit(verb, uri);

            using (var httpClient = new HttpClient()) {
                Menus.PrintEachLetterToConsole("Building HTTP Headers.....");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Connection.OAUTHTOKEN);
                Menus.DisplayHttpHeaders(httpClient.DefaultRequestHeaders.ToString());
                HttpResponseMessage response = null;

                if (postBody != null) {
                    body = JsonConvert.SerializeObject(postBody, Formatting.Indented);
                    Menus.PrintEachLetterToConsole("Setting up the HTTP Request Body.....");
                    Menus.DisplayHttpBody(body);
                }

                Menus.DisplayCompleteRequest(uri,
                    httpClient.DefaultRequestHeaders.Authorization.ToString(),
                    httpClient.DefaultRequestHeaders.Accept.ToString(),
                    verb,
                    body);

                switch (verb) {
                    case "GET":
                        response = await httpClient.GetAsync(uri);
                        break;

                    case "POST":
                        response = await httpClient.PostAsJsonAsync<T>(uri, postBody);
                        break;

                    case "DELETE":
                        response = await httpClient.DeleteAsync(uri);
                        break;

                    case "PATCH":
                        break;

                    default:
                        response = await httpClient.GetAsync(uri);
                        break;
                }

                Menus.PrintEachLetterToConsole("Sending your request.....");
                if (response.IsSuccessStatusCode) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Menus.WriteLineCentered(String.Format("SUCCESS {0} - {1}\n", (int)response.StatusCode, response.ReasonPhrase));
                        Console.ResetColor();
                        Menus.PrintEachLetterToConsole("The server responded with the following Content-Range Header:");
                        Console.WriteLine(response.Content.Headers);
                    try {
                        model = await response.Content.ReadAsAsync<T>();
                    } catch (Exception exc) {
                        Console.WriteLine(exc.Message);
                        throw;
                    }
                    Menus.PressAnyKeyToContinue();
                    return model;
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Menus.PrintBlankSpace(3);
                    Menus.WriteLineCentered(String.Format("FAILURE {0} - {1}", (int)response.StatusCode, response.ReasonPhrase));
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                    Menus.PressAnyKeyToContinue();
                    return default(T);
                }
            }
        }

        #endregion Methods
    }
}