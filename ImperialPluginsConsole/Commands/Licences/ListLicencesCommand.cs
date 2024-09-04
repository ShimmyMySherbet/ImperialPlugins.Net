using ImperialPlugins;
using ImperialPlugins.Models;
using ImperialPlugins.Models.Plugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Collections.Generic;

namespace ImperialPluginsConsole.Commands.Licences
{
    [CommandParent(typeof(LicencesCommand))]
    public class ListLicencesCommand : ICommand
    {
        public string Name => "List";

        public string Syntax => "[-l Limit] [-b|-blocked] [-a|active] [-r|refunded] [-s|special] [-u (user)] [-uid (UserID)] [-p (plugin)] [-pid pluginID] [-ol|OmitLicence] [-fr|ForceRefresh]";

        public string Description => "Lists Licences";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;
        private readonly CacheClient m_Cache;

        public ListLicencesCommand(ImperialPluginsClient imperialPlugins, CommandContext context, CacheClient cache)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
            m_Cache = cache;
        }

        private List<PluginRegistration> Filter(EnumerableResponse<PluginRegistration> registrations, ArgumentList args)
        {
            bool hasTypeFilters = false;

            var blocked = false;
            var active = false;
            var refunded = false;
            var special = false;

            var hasUserFilter = false;
            string? user = null;
            string? userID = null;

            var hasPluginFilter = false;
            int? pluginID = null;

            if (args.ContainsKey("b"))
            {
                hasTypeFilters = true;
                blocked = true;
            }

            if (args.ContainsKey("s"))
            {
                special = true;
                hasTypeFilters = true;
            }

            if (args.ContainsKey("a"))
            {
                hasTypeFilters = true;
                active = true;
            }

            if (args.ContainsKey("r"))
            {
                hasTypeFilters = true;
                refunded = true;
            }

            if (args.ContainsKey("uid"))
            {
                userID = args["uid"];
            }

            if (args.ContainsKey("u"))
            {
                hasUserFilter = true;
                user = args["u"];
            }

            if (args.ContainsKey("pid") && int.TryParse(args["pid"], out var pid))
            {
                hasPluginFilter = true;
                pluginID = pid;
            }

            if (args.ContainsKey("p"))
            {
                hasUserFilter = true;
                var pln = args["p"];
                var pluginInfo = m_Cache.GetPluginByName(pln);
                if (pluginInfo == null)
                {
                    return new List<PluginRegistration>();
                }
                hasPluginFilter = true;
                pluginID = pluginInfo.ID;
            }

            var res = new List<PluginRegistration>();

            foreach (var pl in registrations.Items)
            {
                if (hasTypeFilters)
                {
                    if (pl.IsActive && !active)
                    {
                        continue;
                    }

                    if (pl.IsBlocked && !blocked && !pl.SpecialLicence)
                    {
                        continue;
                    }

                    if (pl.RefundTime != null && !refunded)
                    {
                        continue;
                    }

                    if (pl.SpecialLicence && !special)
                    {
                        continue;
                    }
                }

                if (hasUserFilter)
                {
                    if (userID != null && pl.OwnerID != userID)
                    {
                        continue;
                    }

                    if (user != null && !pl.OwnerName.Contains(user, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                if (hasPluginFilter)
                {
                    if (pluginID != null && pl.ProductID != pluginID)
                    {
                        continue;
                    }
                }

                res.Add(pl);
            }

            return res;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var max = 50;

            var args = m_Context.ArgumentParser
                .WithDependants("u", "uid", "p", "pid", "l")
                .WithIndependants("b", "a", "r")
                .Parse();

            bool showLicence = !args.ContainsKey("ol");
            bool refresh = args.ContainsKey("fr");

            if (args.ContainsKey("l") && !int.TryParse(args["l"], out max))
            {
                cmdOut.WriteLine("Error: Invalid limit", ConsoleColor.Red);
                return;
            }

            var registrations = m_Cache.GetRegistrations(1000, refresh);

            var filtered = Filter(registrations, args);

            var userPad = filtered.GetPadBase(x => x.OwnerName?.Length ?? 0) + 3;

            var dc = new Dictionary<int, IPPlugin?>();

            foreach (var pl in filtered)
            {
                dc[pl.ProductID] = m_Cache.GetPlugin(pl.ProductID);
            }
            var productPad = dc.GetPadBase(x => x.Value?.Name?.Length ?? 5) + 3;

            foreach (var k in filtered)
            {
                var lcid = k.ID.ToString();
                cmdOut.Write("[", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Cyan, lcid);
                cmdOut.Write("]{0}", ConsoleColor.Green, ' '.Pad(lcid.Length, 5));

                cmdOut.Write("User: ", ConsoleColor.Green);
                if (k.OwnerName != null)
                {
                    cmdOut.Write("{0}", ConsoleColor.Cyan, k.OwnerName.Pad(userPad));
                } else
                {
                    cmdOut.Write("{0}", ConsoleColor.Red, "NULL".Pad(userPad));

                }

                cmdOut.Write("Product: ", ConsoleColor.Green);

                var product = m_Cache.GetPlugin(k.ProductID);
                if (product == null)
                {
                    cmdOut.Write("{0}", ConsoleColor.Red, k.ID.ToString().Pad(productPad));
                }
                else
                {
                    cmdOut.Write("{0}", ConsoleColor.Cyan, product.Name.Pad(productPad));
                }

                if (showLicence)
                {
                    cmdOut.Write("Licence Key: ", ConsoleColor.Green);
                    cmdOut.Write(k.LicenseKey, ConsoleColor.Cyan);
                }

                if (k.IsBlocked)
                {
                    cmdOut.Write(" BLOCKED ".Pad(10), ConsoleColor.Red);
                }
                else if (k.IsActive)
                {
                    cmdOut.Write(" ACTIVE".Pad(10), ConsoleColor.Green);
                }
                if (k.RefundTime != null)
                {
                    cmdOut.Write(" REFUNDED".Pad(10), ConsoleColor.Blue);
                }
                cmdOut.WriteLine();

                if (k.ExpireTime != null)
                {
                    cmdOut.Write("  Licence Expired: ", ConsoleColor.Yellow);
                    cmdOut.WriteLine("{0}", ConsoleColor.Cyan, k.ExpireTime.Value.ToShortDateString());
                }

                if (k.IsBlocked)
                {
                    cmdOut.Write("  Blocked Reson: ", ConsoleColor.Yellow);
                    cmdOut.Write("{0}", ConsoleColor.Cyan, k.BlockDisplayReason);
                    cmdOut.WriteLine();
                }
                if (k.RefundTime != null)
                {
                    cmdOut.Write("  Refund Time: ", ConsoleColor.Yellow);
                    cmdOut.WriteLine("{0}", ConsoleColor.Cyan, k.RefundTime.Value.ToShortDateString());
                }
            }
        }
    }
}