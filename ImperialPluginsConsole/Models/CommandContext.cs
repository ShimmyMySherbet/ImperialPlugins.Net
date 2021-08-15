using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Models
{
    public struct CommandContext
    {
        public string CommandName { get; init; }
        public string[] Arguments { get; init; }
        public bool FallbackHandler { get; init; }

        public CommandContext(string commandName, string[] arguments, bool fallback = false)
        {
            CommandName = commandName;
            Arguments = arguments;
            FallbackHandler = fallback;
        }
    }
}
