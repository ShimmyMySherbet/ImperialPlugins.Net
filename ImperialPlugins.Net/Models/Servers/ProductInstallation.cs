using System;

namespace ImperialPlugins.Models.Servers
{
    public class ProductInstallation : IPObject
    {
        public ImperialPluginsClient ImperialPlugins { get; set; }

        public string Host;
        public int Port;
        public int GameServerID;
        public int productRegistrationId;
        public DateTime? whitelistTime;
        public DateTime registrationTime;
        public DateTime lastActivityTime;
        public bool isWhitelisted;
        public string serverName;
        public int id;
    }
}