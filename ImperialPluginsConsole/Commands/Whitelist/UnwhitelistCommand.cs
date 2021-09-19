using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Whitelist
{
    [CommandParent(typeof(WhitelistsCommand))]
    public class SuspendWhitelistCommand : ICommand
    {
        public string Name => "Suspend";

        public string Syntax => "-r [Reg ID] -l [licence key]";

        public string Description => "Revokes a whitelist for a server";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public SuspendWhitelistCommand(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser.WithDependants("r", "l")
                .Parse();
            args.Enforce("r", "l");
            if (!int.TryParse(args["r"], out var regID))
            {
                cmdOut.WriteLine("Invalid reg ID");
                return;
            }
            m_ImperialPlugins.UpdatePluginRegistration(regID, args["l"], null, false);
        }
    }
}