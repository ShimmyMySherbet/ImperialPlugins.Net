using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;

namespace ImperialPluginsConsole.Commands
{
    [DefaultCommand]
    public class FallbackCommand : ICommand
    {
        private readonly CommandContext m_Context;
        private readonly ICommandService m_CommandService;

        public string Name => "";

        public string Syntax => "";
        public string Description => "Default Fallback Command";

        public FallbackCommand(CommandContext context, ICommandService commandService)
        {
            m_Context = context;
            m_CommandService = commandService;
        }

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Failed to find command: ", ConsoleColor.Red);
            cmdOut.WriteLine("'{0}'", ConsoleColor.DarkYellow, m_Context.CommandName);

            var commands = m_CommandService.GetCommands();

            if (commands.Length > 0)
            {
                cmdOut.WriteLine("Available Commands: ");
                var first = true;

                foreach (var cmd in commands)
                {
                    if (first)
                    {
                        cmdOut.Write(cmd.Name, ConsoleColor.Green);
                        first = false;
                    }
                    else
                    {
                        cmdOut.Write(", {0}", ConsoleColor.Green, cmd.Name);
                    }
                }
                cmdOut.WriteLine();
            }
        }
    }
}