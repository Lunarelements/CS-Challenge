using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>Class <c>NameFeed</c> controls the network functionality for the name API.</summary>
    public static class NameFeed
    {
        const string NameAPI = "https://names.privserv.com/api/";

        /// <summary>Makes a network call to get a random name from the name API.</summary>
        /// <param><c>client</c> is the HttpClient used to make the call.</param>
        /// <param><c>printer</c> is use to give the user feedback if something goes wrong.</param>
        /// <returns>A Task<Tuple> containing the random first name and last name.</returns>
        public static async Task<Tuple<string, string>> GetRandomName(HttpClient client, Printer printer)
        {
            try
            {
                string responseBody = await client.GetStringAsync(NameAPI);
                Person jsonResponse = JsonConvert.DeserializeObject<Person>(responseBody);
                return Tuple.Create(jsonResponse.name, jsonResponse.surname);
            }
            catch (HttpRequestException e)
            {
                printer.Value($"There was a problem getting a random name! {e.Message}").PrintToConsole();
                return null;
            }
        }
    }
}