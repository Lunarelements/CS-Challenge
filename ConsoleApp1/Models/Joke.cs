namespace ConsoleApp1
{
    /// <summary>Class <c>Joke</c> models the structure of a joke from the joke API.</summary>
    class Joke
    {
        public string[] categories { get; set; }
        public string created_at { get; set; }
        public string icon_url { get; set; }
        public string id { get; set; }
        public string updated_at { get; set; }
        public string url { get; set; }
        public string value { get; set; }

    }
}
