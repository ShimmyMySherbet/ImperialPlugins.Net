using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands.Whitelist
{
    [CommandParent(typeof(WhitelistsCommand))]
    public class RevokeWhitelistCommand : ICommand
    {
        public string Name => "Revoke";

        public string Syntax => "";

        public string Description => "Tools to revoke whitelists";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.WriteLine($"Available Commands: Server");
        }
    }
}
