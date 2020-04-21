using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>Class <c>Program</c> handles the display for the joke genertaor and its functionality.</summary>
    public class Program
    {
        static string input;
        static string[] categories;
        static string category;
        static Tuple<string, string> name;
        static readonly Printer printer = new Printer();
        static readonly HttpClient client = new HttpClient();

        static int Main(string[] args)
        {
            try
            {
                AsyncMain(args).Wait();
                return 0;
            }
            catch (Exception)
            {
                printer.Value("There was a problem with the application.").PrintToConsole();
                return -1;
            }
        }

        /// <summary>Runs an asyncronous Main for the program.</summary>
        /// <param><c>args</c> is the flags used when the program was started.</param>
        /// <returns>A Task<int> representing if the program ran successfully or not.</returns>
        static async Task<int> AsyncMain(string[] args)
        {
            DisplayFullMenu();
            while (true)
            {
                input = GetTextualInput(null, new[] { "c", "h", "j", "x" }, "That is not a recognized command.");
                switch (input)
                {
                    case "c":
                        // User wants to see the categories available
                        if (categories == null)
                        {
                            categories = await JokeFeed.GetCategories(client, printer);
                        }
                        DisplayCategories(categories);
                        break;
                    case "h":
                        // User wants to see the menu
                        DisplayFullMenu();
                        break;
                    case "j":
                        // User wants to hear some jokes
                        await DisplayJoke();
                        break;
                    case "x":
                        // User wants to leave
                        return 0;
                }
            }
        }

        /// <summary>Prints the full program menu of commands the user can make.</summary>
        public static void DisplayFullMenu()
        {
            printer.Value("***********************************************\n" +
                          "|       Enter c to get joke categories.       |\n" +
                          "|       Enter h to see this menu again.       |\n" +
                          "|            Enter j to get jokes.            |\n" +
                          "|       Enter x to exit the application.      |\n" +
                          "***********************************************"
            ).PrintToConsole();
        }

        /// <summary>Displays the categories available to the user.</summary>
        /// <param><c>categories</c> is the array of categories available.</param>
        public static void DisplayCategories(string [] categories)
        { 
            // Display the categories
            if (categories != null)
            {
                printer.Value("The available categories are:").PrintToConsole();
                printer.PrintArrayToConsole(categories);
            }
        }

        /// <summary>Runs the user through the vaious steps to display a joke.</summary>
        /// <returns>A Task<int> to say the joke was successfully displayed.</returns>
        public static async Task<int> DisplayJoke()
        {
            do
            {
                input = GetTextualInput("Want to use a random name? y/n", new[] { "y", "n" }, "Sorry, I don't understand.");
                if (input == "y")
                {
                    // User wants to use a random name
                    name = await NameFeed.GetRandomName(client, printer);
                }
                else
                {
                    input = GetTextualInput("Want to choose your name? y/n", new[] { "y", "n" }, "Sorry, I don't understand.");
                    if (input == "y")
                    {
                        // User wants to use a custom name
                        printer.Value("Want would you like your first name to be?").PrintToConsole();
                        string firstName = Console.ReadLine();
                        printer.Value("Want would you like your last name to be?").PrintToConsole();
                        string lastName = Console.ReadLine();
                        name = Tuple.Create(firstName, lastName);
                    }
                    else
                    {
                        // User wants to use default name
                        printer.Value("The name used will therefore be Chuck Norris.").PrintToConsole();
                        break;
                    }
                }
            } while (name == null);
            if(name != null)
            {
                printer.Value($"The name is {name.Item1} {name.Item2}").PrintToConsole();
            }


            input = GetTextualInput("Want to specify a category? y/n", new[] { "y", "n" }, "Sorry, I don't understand.");
            if (input == "y")
            {
                // Get categories to remind user and / or verfication
                if (categories == null)
                {
                    categories = await JokeFeed.GetCategories(client, printer);
                }
                do
                {
                    category = GetTextualInput("Enter a category. Enter c to view categories.", categories.Union(new [] { "c" }).ToArray(), "That doesn't seem to be a category.");

                    // User requested to view the categories instead of typing a category
                    if (category[0] == 'c' && category.Length == 1)
                    {
                        DisplayCategories(categories);
                        // Reset input for user to try again
                        category = null;
                    }
                } while (category == null);
            }

            // Ask user how many jokes they want
            int n = GetNumericInput("How many jokes do you want? (1-9)", 1, 9, "That doesn't seem to be a number from 1-9.");

            printer.Value("Please wait while I get your jokes...").PrintToConsole();

            //Get jokes!
            printer.PrintArrayToConsole(await JokeFeed.GetRandomJokes(client, printer, name?.Item1, name?.Item2, category, n));

            // Reset variables for subsequent runs
            name = null;
            category = null;
            categories = null;

            printer.Value("Good stuff! Feel free to go again. Remember to enter h to see the menu.").PrintToConsole();

            return 0;
        }

        /// <summary>Reads in user textual input and makes the call for verification.</summary>
        /// <param><c>prompt</c> is the prompt to tell the user what to input.</param>
        /// <param><c>valid</c> is the array of valid inputs.</param>
        /// <param><c>invalidMessage</c> is a message to give the user feedback if something goes wrong.</param>
        /// <returns>A string of the input.</returns>
        public static string GetTextualInput(string prompt, string[] valid, string invalidMessage)
        {
            string input;
            // Continue to get user input until it is valid
            do
            {
                // Get user input
                if (prompt != null)
                {
                    printer.Value(prompt).PrintToConsole();
                }
                input = Console.ReadLine().Trim();
                input = input.ToLower();
            } while (!ValidateTextualInput(new[] { input }, valid, invalidMessage));

            return input;
        }

        /// <summary>Reads in user numeric input and makes the call for verification.</summary>
        /// <param><c>prompt</c> is the prompt to tell the user what to input.</param>
        /// <param><c>min</c> is the min number of the input.</param>
        /// <param><c>mix</c> is the mix number of the input.</param>
        /// <param><c>invalidMessage</c> is a message to give the user feedback if something goes wrong.</param>
        /// <returns>A int of the input.</returns>
        public static int GetNumericInput(string prompt, int min, int max, string invalidMessage)
        {
            string input;
            // Continue to get user input until it is valid
            do
            {
                // Get user input
                printer.Value(prompt).PrintToConsole();
                input = Console.ReadLine().Trim();
            } while (!ValidateNumericInput(input, min, max, invalidMessage));

            return Int32.Parse(input); ;
        }

        /// <summary>Verfiy user textual input.</summary>
        /// <param><c>input</c> is the input from the user.</param>
        /// <param><c>valid</c> is the array of valid inputs.</param>
        /// <param><c>invalidMessage</c> is a message to give the user feedback if something goes wrong.</param>
        /// <returns>A bool of if the input is valid.</returns>
        public static bool ValidateTextualInput(string[] input, string[] valid, string invalidMessage)
        {
            if (input == null || valid == null)
            {
                printer.Value(invalidMessage).PrintToConsole();
                return false;
            }

            // See if input exists in valid
            foreach (string value in input)
            {
                if (Array.IndexOf(valid, value) == -1)
                {
                    printer.Value(invalidMessage).PrintToConsole();
                    return false;
                }
            }
            return true;
        }

        /// <summary>Verfiy user numeric input.</summary>
        /// <param><c>input</c> is the input from the user.</param>
        /// <param><c>min</c> is the min number of the input.</param>
        /// <param><c>mix</c> is the mix number of the input.</param>
        /// <param><c>invalidMessage</c> is a message to give the user feedback if something goes wrong.</param>
        /// <returns>A bool of if the input is valid.</returns>
        public static bool ValidateNumericInput(string input, int min, int max, string invalidMessage)
        {
            if (input == null)
            {
                printer.Value(invalidMessage).PrintToConsole();
                return false;
            }

            // Check to see if string is actually a number
            if (Int32.TryParse(input, out int number))
            {
                // Check if number is in the range asked
                if (number >= min && number <= max)
                {
                    return true;
                }
            }
            
            printer.Value(invalidMessage).PrintToConsole();
            return false;
        }
    }
}
