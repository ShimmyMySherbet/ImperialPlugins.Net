using System;
using System.Collections.Generic;
using System.Linq;
using ImperialPlugins;
using ImperialPlugins.Models;
using ImperialPlugins.Models.Servers;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using ImperialPluginsConsole.Models.IPLookup;

namespace ImperialPluginsConsole.Commands.Customers
{
    [CommandParent(typeof(CustomersCommand))]
    public class CustomerServersCommand : ICommand
    {
        public string Name => "Servers";

        public string Syntax => "{ -c [Customer] | -ip [Ip] } -p [port] -max [max] -i -d [days]";

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
                .WithDependants("c", "max", "d", "ip", "p")
                .WithIndependants("i")
                .Parse();

            var customer = ctx.GetOrDefault<string?>("c", null);
            var serverIP = ctx.GetOrDefault<string?>("ip", null);
            var serverPort = ctx.GetOrDefault<ushort?>("p", null);

            if (customer == null && serverIP == null)
            {
                cmdOut.Write("Error: ", ConsoleColor.Red);
                cmdOut.WriteLine("-c or -ip must be specified.", ConsoleColor.Yellow);
                return;
            }

            ctx.Enforce("c");

            var maxCount = ctx.GetOrDefault("max", 500);

            var includeIpInfo = ctx.If("i");

            var dayMax = ctx.GetOrDefault("d", 10000);

            EnumerableResponse<ProductInstallation> res;


            if (customer != null)
            {
                var user = m_Cache.GetUser(customer);
                if (user == null)
                {
                    cmdOut.WriteLine("Failed to find user.");
                    return;
                }
                var customerID = user.Id;
                res = m_Client.GetInstallations(customerID, maxCount);

            } else
            {
                return;
            }



            if (res.TotalCount == 0)
            {
                cmdOut.WriteLine("Customer has no servers.");
                return;
            }

            res.Items = res.Items.OrderByDescending(x => x.lastActivityTime).Where(x => DateTime.Now.Subtract(x.lastActivityTime).TotalDays <= dayMax).ToArray();

            var hostNames = new string[res.Items.Length];
            var productNames = new string[res.Items.Length];
            var regDates = new string[res.Items.Length];
            var lastSeenDates = new string[res.Items.Length];

            var ipLookups = new Dictionary<string, IPAPIResponse?>();
            var locPad = 0;
            var ispPad = 0;
            var orgPad = 0;
            if (includeIpInfo)
            {
                ipLookups = IPAPI.LookupIP(res.Items.Select(x => x.Host).ToArray());

                locPad = ipLookups.GetPadBase(x => x.Value != null ? $"{x.Value.regionName}, {x.Value.country}".Length : 0) + 2;
                ispPad = ipLookups.GetPadBase(x => x.Value != null ? x.Value.Isp.Length : 0) + 2;
                orgPad = ipLookups.GetPadBase(x => x.Value != null ? x.Value.Org.Length : 0) + 2;
            }

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

            if (res.Items.Length == 0)
            {
                cmdOut.WriteLine("No servers matching search criteria", ConsoleColor.Red);
                cmdOut.WriteLine();
            }

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

                if (ipLookups.TryGetValue(server.Host, out var ipInf) && ipInf != null && ipInf.IsSuccess)
                {
                    cmdOut.Write("  Location: ", ConsoleColor.Blue);
                    cmdOut.Write($"{ipInf.regionName}, {ipInf.country}".Pad(locPad), ConsoleColor.Yellow);
                    cmdOut.Write("  ISP: ", ConsoleColor.Blue);
                    cmdOut.Write(ipInf.Isp.Pad(ispPad), ConsoleColor.Cyan);
                    cmdOut.Write("  Org: ", ConsoleColor.Blue);

                    var reseller = HosterOverrides.GetResellerName(server.Host);
                    if (reseller != null)
                    {
                        cmdOut.Write(reseller, ConsoleColor.Yellow);
                        cmdOut.Write(" via ", ConsoleColor.Blue);
                        cmdOut.WriteLine(ipInf.Org, ConsoleColor.Magenta);
                    }
                    else
                    {
                        cmdOut.WriteLine(ipInf.Org.Pad(orgPad), ConsoleColor.Yellow);
                        cmdOut.WriteLine();
                    }
                }

                cmdOut.WriteLine();
            }
        }
    }
}