using ImperialPlugins;
using ImperialPlugins.Models.Plugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands.Whitelist
{
    [CommandParent(typeof(RevokeWhitelistCommand))]
    public class RevokeAllCommand : ICommand
    {
        public string Name => "Server";

        public string Syntax => "-a [Address] -f -p [plugin]";

        public string Description => "Revokes whitelisting for a server";

        private readonly CommandContext m_Context;
        private readonly ImperialPluginsClient m_Client;
        private readonly CacheClient m_Cache;

        public RevokeAllCommand(CommandContext context, ImperialPluginsClient client, CacheClient cache)
        {
            m_Context = context;
            m_Client = client;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser
                .WithDependants("a", "p")
                .WithIndependants("f")
                .Parse();

            args.Enforce("a");


            var host = args["a"];
            var force = args.If("f");
            string IP;
            int port = -1;
            if (host.Contains(":"))
            {
                IP = host.Split(":")[0];
                var hostStr = host.Remove(0, IP.Length + 1);

                if (!int.TryParse(hostStr, out port))
                {
                    cmdOut.WriteLine("Invalid port.", ConsoleColor.Red);
                    return;
                }
            }
            else
            {
                IP = host;
            }

            if (port == -1 && !force)
            {
                cmdOut.Write("Failed to revoke whitelist: ", ConsoleColor.Red);
                cmdOut.WriteLine("No port specified. Specify a port or use option -f to revoke all whitelists on this IP address", ConsoleColor.Yellow);
                return;
            }





            var revokes = m_Client.WhitelistServer(host, false, port);


            if (revokes.Length == 0)
            {
                cmdOut.Write("Failed to revoke whitelist: ", ConsoleColor.Red);
                cmdOut.WriteLine("No known whitelists on that address.", ConsoleColor.Yellow);
                return;
            }

            cmdOut.WriteLine($"Revoked {revokes.Length} whitelists.");
            m_Cache.RefreshReg();
        }
    }
}
