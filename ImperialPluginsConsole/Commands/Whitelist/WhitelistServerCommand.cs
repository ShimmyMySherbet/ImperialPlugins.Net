using System;
using System.Collections.Generic;
using System.Linq;
using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Whitelist
{
    [CommandParent(typeof(WhitelistsCommand))]
    public class RevokeServerWhitelistCommand : ICommand
    {
        public string Name => "Server";

        public string Syntax => "-a [address] -f";

        public string Description => "Whitelists a server";

        private readonly CommandContext m_ctx;
        private readonly ImperialPluginsClient m_Client;
        private readonly CacheClient m_Cache;

        public RevokeServerWhitelistCommand(CommandContext ctx, ImperialPluginsClient client, CacheClient cache)
        {
            m_ctx = ctx;
            m_Client = client;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_ctx.ArgumentParser
                .WithDependants("a")
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
                cmdOut.Write("Failed to whitelist: ", ConsoleColor.Red);
                cmdOut.WriteLine("No port specified. Specify a port or use option -f to whitelist all servers on the IP address", ConsoleColor.Yellow);
                return;
            }

            var whitelists = m_Client.WhitelistServer(IP, true, port);

            if (whitelists.Length == 0)
            {
                cmdOut.WriteLine($"No known servers with that address.", ConsoleColor.Yellow);
                return;
            }

            cmdOut.Write("Whietlisted ", ConsoleColor.Green);
            cmdOut.Write(whitelists.Length, ConsoleColor.Cyan);
            cmdOut.WriteLine("Servers.", ConsoleColor.Green);
            cmdOut.WriteLine();

            var namePad = whitelists.GetPadBase(x => x.ServerName.Length) + 2;

            var groups = new List<_WSG>();

            for (int i = 0; i < whitelists.Length; i++)
            {
                var w = whitelists[i];
                _WSG? W;
                W = groups.FirstOrDefault(x => x.Host == w.Host && x.Port == w.Port);
                if (W == null)
                {
                    W = new _WSG(w.Host, w.Port, w.LastActivityTime);
                    groups.Add(W);
                }

                var product = m_Cache.GetPlugin(w.ProductID);
                if (product != null)
                {
                    W.Products.Add(product.Name);
                }
                else
                {
                    W.Products.Add("Unknown Plugin");
                }
            }

            m_Cache.RefreshReg();
        }

        private class _WSG
        {
            public string Host;
            public int Port;

            public List<string> Products = new List<string>();
            public DateTime? LastSeen;

            public _WSG(string host, int port, DateTime? lastSeen)
            {
                Host = host;
                Port = port;
                LastSeen = lastSeen;
            }
        }
    }
}