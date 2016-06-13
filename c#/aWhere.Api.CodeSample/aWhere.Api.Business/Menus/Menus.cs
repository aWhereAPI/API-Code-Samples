using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace aWhere.Api.ConsoleDemo {

    public enum RequestType {
        GetFields = 1,
        GetFieldById = 2,
        PostFields = 3,
        DeleteFields = 4,
        GetObservedData = 5,
        GetForecast = 6,
        GetNorms = 7,
        Continue = 8,
        Quit = 9
    }

    public static class Menus {
        #region Methods

        public static void DisplayCompleteRequest(string host, string authorization, string accept, string verb, string body) {
            Console.WriteLine();
            Menus.PrintEachLetterToConsole(String.Format("Completed HTTP {0} Request Payload:", verb));
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0} {1}", verb, host);
            Console.WriteLine("Host: {0}", host);
            Console.WriteLine("Authorization: {0}", authorization);
            Console.WriteLine("Content-Type: {0}", accept);
            Console.WriteLine("Body: {0}", body);
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void DisplayContinueToMainMenu() {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLineCentered("Press any key to continue to the main menu");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void DisplayHttpBody(string body) {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Body: {0}", body);
            Console.ResetColor();
            Console.WriteLine();
        }

        //public static void DisplayHttpBody(

        public static void DisplayHttpHeaders(string httpClient) {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(httpClient);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void DisplayInit(string verb, string uri) {
            Console.Write("> Initalizing [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(String.Format("{0}", verb));
            Console.ResetColor();
            Console.Write("] Request to ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(String.Format("{0}", uri));
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void DisplayRequestMenu() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            string title = "Select a HTTP Request option for one the following endpoints:";
            WriteLineCentered(title);
            Console.ResetColor();
            DrawMenuOptions();
        }

        public static void DisplayStartupMenu() {
            Console.WindowWidth = 120;
            Console.WindowHeight = 50;
            Console.WriteLine(DrawLogo());
            PrintBlankSpace(2);
            WriteLineCentered("Welcome to the aWhere API Demo!");
            Console.WriteLine();

            WriteLineCentered("This application is intended to show you some basic functionality of the aWhere Developer API Platform.\n");
            WriteLineCentered("You'll need to have already created a Developer profile via our Developer Community.");
            PrintBlankSpace(2);
            Console.Write("\t\tYou can quickly get started by visiting: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("https://developer.awhere.com/api/get-started.");
            Console.ResetColor();
            PrintBlankSpace(2);
            WriteLineCentered("You will be provided a set of API Keys once you have created your app through the Developer Community.");
            WriteLineCentered("You will use your provided API keys to generate an access token.");
            WriteLineCentered("The token is what is sent with regular API calls to authorize your use.");
            PrintBlankSpace(2);
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLineCentered("Press any key to continue!");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void DrawCenteredTitle(string titleText) {
            WriteLineCentered(titleText);
            StringBuilder dashesBelowWord = new StringBuilder();

            for (int i = 0; i < titleText.Length; i++) {
                dashesBelowWord.Append("_");
            }
            WriteLineCentered(dashesBelowWord.ToString());
            Console.WriteLine();
        }

        public static void DrawDashes(int textLength) {
            for (int i = 0; i < textLength + 6; i++) {
                Console.Write("*");
            }
            Console.WriteLine();
        }

        public static string DrawLogo() {
            string logo = @"                          ________  ___       __   ___  ___  _______   ________  _______
                         |\   __  \|\  \     |\  \|\  \|\  \|\  ___ \ |\   __  \|\  ___ \
                         \ \  \|\  \ \  \    \ \  \ \  \\\  \ \   __/|\ \  \|\  \ \   __/|
                          \ \   __  \ \  \  __\ \  \ \   __  \ \  \_|/_\ \   _  _\ \  \_|/__
                           \ \  \ \  \ \  \|\__\_\  \ \  \ \  \ \  \_|\ \ \  \\  \\ \  \_|\ \
                            \ \__\ \__\ \____________\ \__\ \__\ \_______\ \__\\ _\\ \_______\
                             \|__|\|__|\|____________|\|__|\|__|\|_______|\|__|\|__|\|_______|
                                                                                              ";
            return logo;
        }

        public static void DrawMenuOptions() {
            Console.WriteLine();
            Console.WriteLine();
            DrawCenteredTitle("Fields API - Register and Manage your Fields");
            Console.WriteLine();
            List<string> menuOptions = new List<string>();

            menuOptions.Add(String.Format("{0}. [GET] Get all Fields associated with your Account", (int)RequestType.GetFields));
            menuOptions.Add(String.Format("{0}. [GET] Get a specific Field by ID", (int)RequestType.GetFieldById));
            menuOptions.Add(String.Format("{0}. [POST] Create a Field Location", (int)RequestType.PostFields));
            menuOptions.Add(String.Format("{0}. [DELETE] Delete a Field Location", (int)RequestType.DeleteFields));

            foreach (var choice in menuOptions) {
                Console.WriteLine(String.Format("\t\t\t\t\t{0}", choice));
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            menuOptions.Clear(); // Clear the List of Options
            DrawCenteredTitle("Weather API - Provides access to aWhere's agriculture-specific Weather Terrain system");
            Console.WriteLine();
            menuOptions.Add(String.Format("{0}. [GET] Retrieve Daily Observed Weather Data", (int)RequestType.GetObservedData));
            menuOptions.Add(String.Format("{0}. [GET] Retrieve Today's Forecast (+7 days)", (int)RequestType.GetForecast));
            menuOptions.Add(String.Format("{0}. [GET] Retrieve Past 3 years Weather Norms", (int)RequestType.GetNorms));

            foreach (var choice in menuOptions) {
                Console.WriteLine(String.Format("\t\t\t\t\t{0}", choice));
            }
        }

        public static void DrawMenuOptions(RequestType requestType) {
            Console.Clear();
            List<string> menuDescriptions = new List<string>();
            Console.WriteLine();

            switch (requestType) {
                case (RequestType.GetFields):
                    DrawCenteredTitle("Fields API - Perform a GET Request (Field List Endpoint)");
                    menuDescriptions.Add("Use this API to retrieve the fields associated with your account.\n");
                    menuDescriptions.Add("The Field API URI  is https://api.awhere.com/v2/fields/. \n");
                    menuDescriptions.Add("By default, 50 fields will be included in each page of the list, but you can change this with query string parameters.");
                    break;

                case (RequestType.GetFieldById):
                    DrawCenteredTitle("Field API - Perform a GET Request (Single Field Endpoint)");
                    menuDescriptions.Add("You can easily obtain information regarding an individual field by changing the URI of your GET request.");
                    menuDescriptions.Add("The Field API URI  is https://api.awhere.com/v2/fields/. \n");
                    menuDescriptions.Add("For this demo, you'll type in the name of the Field you wish to perform a GET request on.");
                    break;

                case (RequestType.DeleteFields):
                    DrawCenteredTitle("Field API - Perform a DELETE Request");
                    menuDescriptions.Add("Occasionally it is necessary to remove a field from your account, such as when you need to refine the geolocation.");
                    menuDescriptions.Add("This API allows you to delete a field.");
                    break;

                case (RequestType.PostFields):
                    DrawCenteredTitle("Field API - Perform a POST Request");
                    menuDescriptions.Add("Creating a field registers the location with the aWhere system, ");
                    menuDescriptions.Add("making it easier to reference and track your locations as well as run agronomics and models automatically.\n\n");
                    menuDescriptions.Add("Let's generate a new Field with some random data.");
                    break;

                case (RequestType.GetObservedData):
                    DrawCenteredTitle("Weather API - Daily Observed Weather Info");
                    menuDescriptions.Add("Another feature of the Weather API is the Observation API which allows you to access up to 30 months of data.");
                    menuDescriptions.Add("This API opens the weather attributes that matter most to agriculture.");
                    menuDescriptions.Add("For this demo, you'll get back 7 days of data for the selected field location.");
                    break;

                case (RequestType.GetForecast):
                    DrawCenteredTitle("Weather API Forecast GET Request Info");
                    menuDescriptions.Add("Use this API to return today's forecast plus the forecast for up to 7 more days.");
                    menuDescriptions.Add("Forecasts are available in many sizes of hourly blocks, from hourly to daily.");
                    menuDescriptions.Add("For this demo, we will return today's forecast plus the next 7 days worth of data for the selected field location.");
                    break;

                case (RequestType.GetNorms):
                    DrawCenteredTitle("Weather API Norms GET Request Info");
                    menuDescriptions.Add("This API that allows you to calculate the averages for weather attributes across any range of years for which we have data");
                    menuDescriptions.Add("Whereas the Daily Observed API only supports up to 30 months of daily data, this API allow you to compare this year and the previous year to the long-term normals (however many years you want to include).\n");
                    menuDescriptions.Add("Each day's worth of data also includes the standard deviation for the average\n");
                    menuDescriptions.Add("For this demo, we will be retrieving the average of the last 10 years for a single day (06/01/2016)");
                    
                    break;

                default:
                    break;
            }

            foreach (string description in menuDescriptions) {
                WriteLineCentered(description);
            }

            menuDescriptions.Clear();
            Console.WriteLine();
            //DrawCenteredTitle("Here are the current Fields associated with your Account:");
            PressAnyKeyToContinue();
            Console.WriteLine();
        }

        public static void PressAnyKeyToContinue() {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLineCentered("Press any key to continue....");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void PrintBlankSpace(int p) {
            for (int i = 0; i < p; i++) {
                Console.WriteLine();
            }
        }

        public static void PrintEachLetterToConsole(string words) {
            if(String.IsNullOrWhiteSpace(words)){
                throw new ArgumentException("Input cannot be empty/null", "words");
            }

            Console.Write("> ");
            foreach (char c in words)
	        {
		        Console.Write(c);
                Thread.Sleep(15);
	        }

            Console.WriteLine();
        }

        public static void WriteCentered(string textToCenter) {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (textToCenter.Length / 2)) + "}", textToCenter));
        }

        public static void WriteLineCentered(string textToCenter) {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (textToCenter.Length / 2)) + "}", textToCenter));
        }

        public static void WriteLineWordWrap(string paragraph, int tabSize = 8) {
            string[] lines = paragraph
                .Replace("\t", new String(' ', tabSize))
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++) {
                string process = lines[i];
                List<String> wrapped = new List<string>();

                while (process.Length > Console.WindowWidth) {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0) break;

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped) {
                    Console.WriteLine(wrap);
                }

                Console.WriteLine(process);
            }
        }

        public static void WriteOnBottomLine(string text) {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.Write(text);
            // Restore previous position
            Console.SetCursorPosition(x, y);
        }

        #endregion Methods
    }
}