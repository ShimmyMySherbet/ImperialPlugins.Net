using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands.Licences
{
    public class LicencesCommand : ICommand
    {
        public string Name => "Licences";

        public string Syntax => "";

        public string Description => "Manages licences and product registrations";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("Block, Unblock, List", ConsoleColor.Yellow);
        }
    }
}