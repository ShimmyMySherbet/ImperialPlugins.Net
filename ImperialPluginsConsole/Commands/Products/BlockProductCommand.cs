using ImperialPlugins;
using ImperialPluginsConsole.Commands.Products;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;

namespace ImperialPluginsConsole.Commands.Products
{
    [CommandParent(typeof(ProductCommand))]
    public class BlockProductCommand : ICommand
    {
        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public string Name => "Block";

        public string Syntax => "-l [Licence] -r [Reason]";

        public string Description => "Blocks a licence key for the specified reason";

        public BlockProductCommand(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser.WithDependants("l", "r").Parse();
            args.Enforce("l", "r");

            var licence = args["l"];
            var reason = args["r"];

            if (string.IsNullOrEmpty(licence))
            {
                cmdOut.WriteLine("Licence key must not be empty", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrEmpty(reason))
            {
                cmdOut.WriteLine("Reason must not be empty", ConsoleColor.Red);
                return;
            }

            m_ImperialPlugins.BlockProduct(licence, reason);
        }
    }
}