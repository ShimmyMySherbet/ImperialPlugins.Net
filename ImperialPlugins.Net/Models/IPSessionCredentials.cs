namespace ImperialPlugins.Models
{
    public struct IPSessionCredentials
    {
        public IPSessionCredentials(string type, string access)
        {
            AuthHeaderContent = type + " " + access;
            Header = "authorization";
        }

        public IPSessionCredentials(string APIKey)
        {
            Header = "X-API-KEY";
            AuthHeaderContent = APIKey;
        }

        public string Header { get; set; }

        public string AuthHeaderContent { get; set; }
    }
}