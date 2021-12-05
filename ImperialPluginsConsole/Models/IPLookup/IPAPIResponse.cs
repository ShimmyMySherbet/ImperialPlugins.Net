using Newtonsoft.Json;

namespace ImperialPluginsConsole.Models.IPLookup
{
    public class IPAPIResponse
    {
        [JsonIgnore]
        public bool IsSuccess => !string.IsNullOrEmpty(status) && status == "success";

        public string status = string.Empty;

        public string message = string.Empty;

        public string country = string.Empty;

        public string regionName = string.Empty;

        public string city = string.Empty;

        public string Isp = string.Empty;
        public string Org = string.Empty;

        public bool Hosting = false;

        public string Query = string.Empty;

    }
}