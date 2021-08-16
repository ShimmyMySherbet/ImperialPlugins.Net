using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands
{
    public class UnreadMessagesCountCommand : ICommand
    {
        private readonly ImperialPluginsClient m_ImperialPlugins;

        public UnreadMessagesCountCommand(ImperialPluginsClient imperialPlugins)
        {
            m_ImperialPlugins = imperialPlugins;
        }

        public string Name => "UnreadMessages";

        public string Syntax => "";

        public string Description => "Checks how many unread messages you have";

        public void Execute(ICommandOut cmdOut)
        {
            var count = m_ImperialPlugins.UnreadMessages();

            cmdOut.Write("You have ", ConsoleColor.Green);
            cmdOut.Write($"{count}", ConsoleColor.Cyan);
            cmdOut.WriteLine(" unread messages.", ConsoleColor.Green);
        }
    }
}