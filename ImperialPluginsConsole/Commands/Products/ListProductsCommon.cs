using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Linq;

namespace ImperialPluginsConsole.Commands.Products
{
    [CommandParent(typeof(ProductsCommand))]
    public class ListProductsCommon : ICommand
    {
        public string Name => "List";

        public string Syntax => "[-m Merchant] [-l limit]";

        public string Description => "Lists store products";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;
        private readonly CacheClient m_Cache;

        public ListProductsCommon(ImperialPluginsClient imperialPlugins, CommandContext context, CacheClient cache)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser
                .WithDependants("m", "l")
                .Parse();

            var products = m_Cache.GetPlugins(200);

            string? merchant = null;

            if (args.ContainsKey("m"))
            {
                merchant = args["m"];
            }

            var mPad = products.Items.Max(x => x.Merchant.Name.Length) + 2;
            var pPad = products.Items.Max(x => x.Name.Length) + 2;


            foreach (var pl in products.Items)
            {
                if (merchant != null && !pl.Merchant.Name.Contains(merchant, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                cmdOut.Write("[", ConsoleColor.Green);
                cmdOut.Write($"{pl.ID}", ConsoleColor.Cyan);
                cmdOut.Write("] ", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Cyan, pl.Name.Pad(pPad));

                cmdOut.Write(" Merchant: ", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Cyan, pl.Merchant.Name.Pad(mPad));

                cmdOut.Write(" Status: ", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Cyan, pl.Status.Pad(10));

                cmdOut.Write(" Price: ", ConsoleColor.Green);
                cmdOut.Write("${0}", ConsoleColor.Cyan, pl.UnitPrice.ToString().Pad(5));

                if (pl.IsPublic)
                {
                    cmdOut.Write(" Public", ConsoleColor.White);
                }
                else
                {
                    cmdOut.Write(" Private", ConsoleColor.Blue);
                }

                if (pl.IsCurated)
                {
                    cmdOut.Write(" Curated", ConsoleColor.Yellow);
                }
                cmdOut.WriteLine();

                cmdOut.Write("  Description: ", ConsoleColor.Green);
                cmdOut.WriteLine("{0}", ConsoleColor.Yellow, pl.ShortDescription);
                cmdOut.WriteLine();
            }
        }
    }
}