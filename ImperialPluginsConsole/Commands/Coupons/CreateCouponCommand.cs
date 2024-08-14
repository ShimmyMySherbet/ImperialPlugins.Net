using System;
using System.Linq;
using System.Threading.Tasks;
using ImperialPlugins;
using ImperialPlugins.Models.Coupons;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Coupons
{
    [CommandParent(typeof(CouponCommand))]
    public class CreateCouponCommand : AsyncCommand
    {
        public override string Name => "Create";
        public override string Syntax => "Create";
        public override string Description => "Creates a discount coupon";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;
        private readonly CacheClient m_Cache;

        public CreateCouponCommand(ImperialPluginsClient imperialPlugins, CommandContext context, CacheClient cache)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
            m_Cache = cache;
        }

        public override async Task ExecuteAsync(ICommandOut cmdOut)
        {
            var ctx = m_Context.ArgumentParser
                .WithDependants("products", "discount", "name", "code", "discount", "maxUses")
                .WithIndependants("i")
                .Parse();

            ctx.Enforce("code", "discount");

            var coupon = new CouponBuilder();

            coupon.Name = ctx.GetOrThrow<string>(["name", "code"]);
            coupon.Discount = ctx.GetOrThrow<float>("discount");
            coupon.Key = ctx.GetOrThrow<string>("code");

            coupon.Products = ctx.GetOrDefault("products", string.Empty)
                                    .Split(',')
                                    .Map<string, int>(StringParser.TryParse)
                                    .ToList();

            coupon.MaxUsages = ctx.GetOrDefault("MaxUsages", 0);

            coupon.IsGlobal = false;
            coupon.IsEnabled = true;
            coupon.IsMerchantGlobal = coupon.Products.Count == 0;

            await m_ImperialPlugins.CreateCouponAsync(coupon);

            cmdOut.Write("Created Coupon: ", ConsoleColor.Green);
            cmdOut.WriteLine(coupon.Name, ConsoleColor.Green);
        }
    }
}
