using ImperialPluginsConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands.Whitelist
{
    public class WhitelistsCommand : ICommand
    {
        public string Name => "Whitelist";

        public string Syntax => "";

        public string Description => "Manages Whitelist-based DRM";

        public void Execute(ICommandOut cmdOut)
        {
        }
    }
}
