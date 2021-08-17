using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands.Merchants
{
    public class MerchantsCommand : ICommand
    {
        public string Name => "Merchants";

        public string Syntax => "";

        public string Description => "Tools to view and manage merchants";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available Commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("List", ConsoleColor.Cyan);
        }
    }
}