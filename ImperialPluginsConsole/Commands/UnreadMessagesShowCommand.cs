using ImperialPluginsConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImperialPluginsConsole.Models.Attributes;
namespace ImperialPluginsConsole.Commands
{
    [CommandParent(typeof(UnreadMessagesCountCommand))]
    public class UnreadMessagesShowCommand : ICommand
    {
        public string Name => "Show";

        public string Syntax => "";

        public string Description => "Fetches all unread messages";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.WriteLine("Not Implemented", ConsoleColor.Red);
        }
    }
}
