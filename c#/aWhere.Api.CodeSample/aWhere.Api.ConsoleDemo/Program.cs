using System;
using System.Net;
using System.Threading.Tasks;
using aWhere.Api.Services;

namespace aWhere.Api.ConsoleDemo {
    public class Program {
        #region Constructors

        public Program() {
            Console.Title = "aWhere API Demo";
            QuitRequested = false;
        }

        #endregion Constructors

        #region Constants

        /// <summary>
        ///  Enter your API Keys here. You'll obtain your API Keys by creating an app through our Developer Community. You can visit
        ///  the community here: http://developer.awhere.com/api/get-started
        /// </summary>
        public const string API_KEY = "";

        public const string API_SECRET = "";

        #endregion Constants

        #region Properties

        public aWhereApiConnection ApiConnection { get; set; }
        public bool QuitRequested { get; set; }
        public aWhereRepository Repository { get; set; }
        public ConsoleKeyInfo UserInput { get; set; }

        #endregion Properties

        #region Methods

        private static void Main(string[] args) {
            MainAsync().Wait();
        }

        private static async Task MainAsync() {
            //Specify TLS 1.2, which is required.  See http://developer.awhere.com/api/calling-api-tls-12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Program app = new Program();
            Menus.DisplayStartupMenu();
            app.AuthenticateUser(API_KEY, API_SECRET); // Enter your API Keys in the "Constants" region above
            Menus.DisplayContinueToMainMenu();

            while (!app.QuitRequested && app.ApiConnection.IsAuthenticated) {
                Menus.DisplayRequestMenu();
                app.UserInput = app.GetUserInput();
                await app.HandleInput(app.UserInput);
                await app.AskUserToContinue();
                Console.Clear();
            }
        }

        private async Task AskUserToContinue() {
            Menus.PrintBlankSpace(3);
            Console.WriteLine(String.Format("Would you like to continue? Press {0} or {1} to quit.", "Y", "N"));
            UserInput = GetUserInput();
            await HandleInput(UserInput);
        }

        private void AuthenticateUser(string api_key, string api_secret) {
            try {
                ApiConnection = new aWhereApiConnection(api_key, api_secret);
            } catch (Exception exc) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exc.InnerException.Message);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Press any key to Exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
            Repository = new aWhereRepository(ApiConnection);
        }

        private ConsoleKeyInfo GetUserInput() {
            Console.WriteLine();
            Console.WriteLine("Please enter a selection. ");
            Console.Write("> ");
            var userIput = Console.ReadKey();

            return userIput;
        }

        private string GetUserLineInput() {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please type in the complete ID of the Field");
            Console.Write("> ");
            var userIput = Console.ReadLine();
            Console.Clear();

            return userIput;
        }

        private async Task HandleInput(ConsoleKeyInfo consoleKeyInfo) {
            //Console.Clear();
            var userChoice = consoleKeyInfo.Key;
            string fieldId = "";

            switch (userChoice) {
                // Handle "1" - GET Request on Fields API
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Menus.DrawMenuOptions(RequestType.GetFields);
                    await Repository.GetAllFieldsAsync();
                    break;

                // Handle "2" - GET Request  on Fields API by Id
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Menus.DrawMenuOptions(RequestType.GetFieldById);
                    await Repository.GetAllFieldsAsync();
                    fieldId = GetUserLineInput();
                    await Repository.GetFieldByIdAsync(fieldId);
                    break;

                // Handle "3" - POST Request on Fields API to Create a Field Location
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Menus.DrawMenuOptions(RequestType.PostFields);
                    await Repository.CreateFieldPostRequestAsync();
                    await Repository.GetAllFieldsAsync();
                    break;

                // Handle "4" - DELETE Request on a Field by ID
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Menus.DrawMenuOptions(RequestType.DeleteFields);
                    await Repository.GetAllFieldsAsync();
                    fieldId = GetUserLineInput();
                    await Repository.DeleteFieldByIdAsync(fieldId);
                    break;

                // Handle "5" - GET Request on Weather API  for Observed Weather Data
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Menus.DrawMenuOptions(RequestType.GetObservedData);
                    await Repository.GetAllFieldsAsync();
                    fieldId = GetUserLineInput();
                    await Repository.GetWeatherObservationsForFieldByIdAsync(fieldId);
                    break;

                // Handle "6" - GET Request on Weather API for Today's Forecast (+7 days)
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    Menus.DrawMenuOptions(RequestType.GetForecast);
                    await Repository.GetAllFieldsAsync();
                    fieldId = GetUserLineInput();
                    await Repository.GetWeatherForecastByIdAsync(fieldId);
                    break;

                // Handle "7" - GET Request on Weather API for 3 year Weather Norms
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    Menus.DrawMenuOptions(RequestType.GetNorms);
                    await Repository.GetAllFieldsAsync();
                    fieldId = GetUserLineInput();
                    await Repository.GetWeatherNormsByIdAsync(fieldId);
                    break;

                // User wants to Quit
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                case ConsoleKey.N:
                    QuitRequested = true;
                    break;

                case ConsoleKey.Y:
                    Console.Clear();
                    break;

                default:
                    Console.WriteLine("\t<--------- Invalid Input. Please try again");
                    consoleKeyInfo = GetUserInput();
                    await HandleInput(consoleKeyInfo);
                    break;
            }
        }

        #endregion Methods
    }
}