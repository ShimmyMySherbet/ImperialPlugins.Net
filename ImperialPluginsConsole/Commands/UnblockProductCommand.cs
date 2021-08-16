using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using System;

namespace ImperialPluginsConsole.Commands
{
    public class UnblockProductCommand : ICommand
    {
        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public UnblockProductCommand(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

        public string Name => "UnblockProduct";

        public string Syntax => "UnblockProduct -l [LicenceKey]";

        public string Description => "Unblocks a licence key";

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser.WithDependants("l").Parse();
            args.Enforce("l");

            var licence = args["l"];

            if (string.IsNullOrEmpty(licence))
            {
                cmdOut.WriteLine("Licence cannot be empty", ConsoleColor.Red);
                return;
            }

            m_ImperialPlugins.UnblockProduct(licence);
        }
    }
}