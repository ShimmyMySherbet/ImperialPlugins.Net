using ImperialPluginsConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands
{
    public class ClearCommand : ICommand
    {
        public string Name => "Clear";

        public string Syntax => "";

        public string Description => "Clears the console";

        public void Execute(ICommandOut cmdOut)
        {
            Console.Clear();
        }
    }
}
