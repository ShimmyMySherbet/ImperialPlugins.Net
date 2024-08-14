using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Coupons
{
    [CommandParent(typeof(CouponCommand))]
    public class CouponsListCommand : AsyncCommand
    {
        public override string Name => "List";
        public override string Syntax => "";
        public override string Description => "Lists coupons";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CacheClient m_Cache;
        public CouponsListCommand(ImperialPluginsClient imperialPlugins, CacheClient cache)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Cache = cache;
        }

        public override async Task ExecuteAsync(ICommandOut cmdOut)
        {
            foreach (var coupon in (await m_ImperialPlugins.GetCouponsAsync(100)).Items)
            {
                cmdOut.Write("[", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Yellow, coupon.ID);
                cmdOut.Write("]{0}", ConsoleColor.Green, ' '.Pad(coupon.ID.ToString().Length, 4));

                cmdOut.Write(coupon.Name.Pad(28), ConsoleColor.Green);

                cmdOut.Write("Code: ", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Cyan, coupon.Key.Pad(16));

                cmdOut.Write("Discount: ", ConsoleColor.Green);

                cmdOut.Write($"{coupon.Discount}%".Pad(5), ConsoleColor.Cyan);

                string statusTerm;
                ConsoleColor statusColor;
                if (coupon.IsEnabled && !coupon.IsActive)
                {
                    statusTerm = "Inactive";
                    statusColor = ConsoleColor.DarkCyan;
                }
                else if (coupon.IsEnabled && coupon.IsActive)
                {
                    statusTerm = "Active";
                    statusColor = ConsoleColor.Green;
                }
                else if (!coupon.IsEnabled && coupon.IsActive)
                {
                    statusTerm = "Suspended";
                    statusColor = ConsoleColor.Yellow;
                }
                else
                {
                    statusTerm = "Expired";
                    statusColor = ConsoleColor.Red;
                }

                cmdOut.Write($" {statusTerm} ".Pad(10), statusColor);

                cmdOut.Write(" Uses: ", ConsoleColor.Green);

                cmdOut.Write($"{coupon.Usages}", ConsoleColor.Cyan);
                cmdOut.Write("/", ConsoleColor.Yellow);
                cmdOut.Write($"{coupon.MaxUsages}".Pad(5, (coupon.Usages.ToString().Length + coupon.MaxUsages.ToString().Length)), ConsoleColor.Cyan);

                cmdOut.Write("    ");

                cmdOut.Write("Start:   ", ConsoleColor.Green);
                if (coupon.StartTime != null)
                {
                    cmdOut.Write($"{coupon.StartTime.Value:yyyy/MM/dd HH:mm}".Pad(20));
                }
                else
                {
                    cmdOut.Write("No Start".Pad(20), ConsoleColor.Yellow);
                }

                cmdOut.Write("Expires: ", ConsoleColor.Green);
                if (coupon.ExpirationTime != null)
                {
                    cmdOut.Write($"{coupon.ExpirationTime.Value:yyyy/MM/dd HH:mm}".Pad(20));
                }
                else
                {
                    cmdOut.Write("No Expiration".Pad(20), ConsoleColor.Yellow);
                }

                cmdOut.Write("  Valid On: ", ConsoleColor.Green);

                var first = true;

                foreach (var pid in coupon.Products)
                {
                    if (!first)
                    {
                        cmdOut.Write(", ", ConsoleColor.Blue);
                    }
                    first = false;



                    var plugin = m_Cache.GetPlugin(pid);

                    cmdOut.Write(plugin?.Name ?? $"Plugin ID {pid}", ConsoleColor.Cyan);
                }
                cmdOut.WriteLine();
            }
        }
    }
}
