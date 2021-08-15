using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Servers
{
    public class Server : IPObject
    {
        public string Host;
        public int Port;
        public List<int> Products;
        public string ServerName;
        public DateTime RegistrationTime;
        public DateTime LastActivityTime;
        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins { get; set; }

        public void WhitelistServer() => ImperialPlugins.WhitelistServer(Host, true, Port);
        public void RevokeWhitelist() => ImperialPlugins.WhitelistServer(Host, false, Port);
    }
}
