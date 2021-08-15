using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Plugins
{
    public class PluginWhitelist
    {
        public int ID;
        public string Host;
        public ushort Port;
        public int ProductID;
        public int ProductRegistrationId;
        public DateTime WhitelistTime;
        public DateTime RegistrationTime;
        public DateTime? LastActivityTime;
        public bool IsWhitelisted;
        public string ServerName;

        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins { get; set; }

        public void AcceptWhitelist() => ImperialPlugins.WhitelistServer(Host, true, Port);
        public void DenyWhitelist() => ImperialPlugins.WhitelistServer(Host, false, Port);

    }
}
