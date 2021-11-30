using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Linq;

namespace ImperialPluginsConsole.Commands.Customers
{
    [CommandParent(typeof(CustomersCommand))]
    public class CustomerServersCommand : ICommand
    {
        public string Name => "Servers";

        public string Syntax => "-c [Customer] -max [max]";

        public string Description => "Lists a customers server registrations";

        private readonly CommandContext m_Context;
        private readonly ImperialPluginsClient m_Client;
        private readonly CacheClient m_Cache;

        public CustomerServersCommand(CommandContext context, ImperialPluginsClient client, CacheClient cache)
        {
            m_Context = context;
            m_Client = client;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var ctx = m_Context.ArgumentParser
                .WithDependants("c", "max")
                .Parse();

            ctx.Enforce("c");

            var customerHandle = ctx["c"];

            var maxCount = ctx.GetOrDefault("max", 500);

            var user = m_Cache.GetUser(customerHandle);

            if (user == null)
            {
                cmdOut.WriteLine("Failed to find user.");
                return;
            }

            var customerID = user.Id;

            var res = m_Client.GetInstallations(customerID, maxCount);

            if (res.TotalCount == 0)
            {
                cmdOut.WriteLine("Customer has no servers.");
                return;
            }

            res.Items = res.Items.OrderByDescending(x => x.lastActivityTime).ToArray();

            var hostNames = new string[res.Items.Length];
            var productNames = new string[res.Items.Length];
            var regDates = new string[res.Items.Length];
            var lastSeenDates = new string[res.Items.Length];

            for (int i = 0; i < res.Items.Length; i++)
            {
                var productID = res.Items[i].productRegistrationId;

                var reg = m_Cache.GetRegistration(productID);
                if (reg == null)
                {
                    productNames[i] = "Unknown";
                    continue;
                }
                var product = m_Cache.GetPlugin(reg.ProductID);
                if (product == null)
                {
                    productNames[i] = "Unknown";
                    continue;
                }
                productNames[i] = product.Name;
            }
            for (int i = 0; i < res.Items.Length; i++)
            {
                var r = res.Items[i];
                regDates[i] = $"{r.registrationTime.ToShortTimeString()} {r.registrationTime.ToShortDateString()}";
                lastSeenDates[i] = $"{r.lastActivityTime.ToShortTimeString()} {r.lastActivityTime.ToShortDateString()}";
                hostNames[i] = $"{r.Host}:{r.Port}";
            }
            var regPadTo = regDates.GetPadBase(x => x.Length) + 2;
            var seenPadTo = lastSeenDates.GetPadBase(x => x.Length) + 2;
            var productPad = productNames.GetPadBase(x => x.Length) + 2;
            var namePad = res.Items.GetPadBase(x => x.serverName.Length) + 2;
            var hostPad = hostNames.GetPadBase(x => x.Length) + 2;
            cmdOut.WriteLine();
            for (int i = 0; i < res.Items.Length; i++)
            {
                var server = res.Items[i];

                cmdOut.Write("[", System.ConsoleColor.Green);
                cmdOut.Write(server.id, System.ConsoleColor.Yellow);
                cmdOut.Write("] ", System.ConsoleColor.Green);

                cmdOut.Write(productNames[i].Pad(productPad), System.ConsoleColor.DarkGreen);

                cmdOut.Write(" Host: ", System.ConsoleColor.Blue);
                cmdOut.Write(hostNames[i].Pad(hostPad), System.ConsoleColor.Cyan);

                cmdOut.Write("Server Name: ", System.ConsoleColor.Blue);
                cmdOut.Write(server.serverName.Pad(namePad), System.ConsoleColor.Cyan);
                cmdOut.Write("[", ConsoleColor.Green);
                if (server.isWhitelisted)
                {
                    cmdOut.Write($"Whitelisted", ConsoleColor.Green);
                }
                else
                {
                    cmdOut.Write($"Not Whitelisted", ConsoleColor.Red);
                }
                cmdOut.Write("]", ConsoleColor.Green);
                cmdOut.WriteLine();
                cmdOut.Write("  Registered: ", System.ConsoleColor.Blue);
                cmdOut.Write(regDates[i], regPadTo, ConsoleColor.Cyan);

                cmdOut.Write("  Seen: ", System.ConsoleColor.Blue);
                cmdOut.Write(lastSeenDates[i], seenPadTo, ConsoleColor.Cyan);
             
                cmdOut.WriteLine();
                cmdOut.WriteLine();
            }
        }
    }
}