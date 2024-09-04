using System;
using System.Threading.Tasks;
using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Coupons
{
    [CommandParent(typeof(CouponCommand))]
    public class CouponDelete : AsyncCommand
    {
        public override string Name => "Delete";
        public override string Syntax => "-id [Coupon ID]";
        public override string Description => "Deletes a coupon";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public CouponDelete(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

        public override async Task ExecuteAsync(ICommandOut cmdOut)
        {
            var ctx = m_Context.ArgumentParser
                     .WithDependants("id")
                     .Parse();

            if (!ctx.If("id"))
            {
                cmdOut.Write("Usage: ", ConsoleColor.Green);
                cmdOut.Write("Coupons Delete", ConsoleColor.Yellow);

                cmdOut.Write(" -id ", ConsoleColor.Blue);

                cmdOut.Write("[", ConsoleColor.Yellow);
                cmdOut.Write("Coupon ID", ConsoleColor.Cyan);
                cmdOut.WriteLine("]", ConsoleColor.Yellow);
                return;
            }

            var couponID = ctx.GetOrThrow<int>("id");
            await m_ImperialPlugins.DeleteCouponAsync(couponID);

            cmdOut.Write("Deleted Coupon: ", ConsoleColor.Green);
            cmdOut.WriteLine(couponID, ConsoleColor.Green);
        }
    }
}
