using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands.Customers
{
    public class CustomersCommand : ICommand
    {
        public string Name => "Customers";

        public string Syntax => "";

        public string Description => "Lists customers";

        private ImperialPluginsClient m_ImperialPlugins;

        public CustomersCommand(ImperialPluginsClient imperialPlugins)
        {
            m_ImperialPlugins = imperialPlugins;
        }

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("List, Servers", ConsoleColor.Yellow);
        }
    }
}