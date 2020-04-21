using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>Class <c>JokeFeed</c> controls the network functionality for the joke API.</summary>
    public static class JokeFeed
    {
        const string ChuckNorrisAPI = "https://api.chucknorris.io/jokes/";

        /// <summary>Makes a network call to get the categories from the joke API.</summary>
        /// <param><c>client</c> is the HttpClient used to make the call.</param>
        /// <param><c>printer</c> is use to give the user feedback if something goes wrong.</param>
        /// <returns>A Task for a string array of all the categories available.</returns>
        public static async Task<string[]> GetCategories(HttpClient client, Printer printer)
        {
            try
            {
                string responseBody = await client.GetStringAsync($"{ChuckNorrisAPI}categories");
                return JsonConvert.DeserializeObject<string[]>(responseBody);
            }
            catch (HttpRequestException e)
            {
                printer.Value($"There was a problem getting the categories! {e.Message}").PrintToConsole();
                return null;
            }
        }

        /// <summary>Makes a network call to get a number of jokes from the joke API.
        /// The name Chuck Norris is replaced if a name is provided.</summary>
        /// <param><c>client</c> is the HttpClient used to make the call.</param>
        /// <param><c>printer</c> is use to give the user feedback if something goes wrong.</param>
        /// <param><c>firstname</c> is the first name requested to be used in the joke.</param>
        /// <param><c>lastname</c> is the last name requested to be used in the joke.</param>
        /// <param><c>category</c> is the category that the jokes should be.</param>
        /// <param><c>numJokes</c> is the number of jokes requested.</param>
        /// <returns>A Task for a string array of all the jokes.</returns>
        public static async Task<string[]> GetRandomJokes(HttpClient client, Printer printer, string firstname, string lastname, string category, int numJokes)
        {
            try
            {
                // Prepare url and add category if there is one
                string url = "random";
                if (category != null)
                {
                    url += $"?category={category}";
                }

                string[] jokes = new string[numJokes];
                // Get as many jokes as was asked for
                for (int i = 0; i < numJokes; i++)
                {
                    Joke jsonResponse = JsonConvert.DeserializeObject<Joke>(await client.GetStringAsync($"{ChuckNorrisAPI}{url}"));
                    jokes[i] = jsonResponse.value;
                    // Replace Chuck Norris with a name if they specified one
                    if (firstname != null && lastname != null)
                    {
                        jokes[i] = jokes[i].Replace("Chuck Norris", $"{firstname} {lastname}");
                    }
                }
                return jokes;
            }
            catch (HttpRequestException e)
            {
                printer.Value($"There was a problem getting the jokes! {e.Message}").PrintToConsole();
                return null;
            }
        }
    }
}