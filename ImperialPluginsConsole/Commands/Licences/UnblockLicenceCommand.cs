using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;

namespace ImperialPluginsConsole.Commands.Licences
{
    [CommandParent(typeof(LicencesCommand))]
    public class UnblockLicenceCommand : ICommand
    {
        public string Name => "Unblock";

        public string Syntax => "[-l LicenceKey]";

        public string Description => "Unblocks a licence key";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public UnblockLicenceCommand(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

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