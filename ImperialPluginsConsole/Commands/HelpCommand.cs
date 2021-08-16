using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using System;
using System.Linq;
using System.Text;
using ImperialPluginsConsole.Models.Attributes;
namespace ImperialPluginsConsole.Commands
{
    [HelpProvider]
    public class HelpCommand : ICommand
    {
        private readonly ICommandService m_CommandService;
        private readonly ICommand? m_Caller;
        private readonly CommandInfo? m_CallerInfo;
        public string Name => "Help";

        public string Syntax => "";

        public string Description => "Shows available commands and their usage";

        public HelpCommand(ICommandService commandService, ICommand caller, CommandInfo callerInfo)
        {
            m_CommandService = commandService;
            m_Caller = caller;
            m_CallerInfo = callerInfo;
        }

        public HelpCommand(ICommandService commandService)
        {
            m_CommandService = commandService;
            m_Caller = null;
            m_CallerInfo = null;
        }

        public void Execute(ICommandOut cmdOut)
        {
            if (m_Caller == null || m_CallerInfo == null)
            {
                var commands = m_CommandService.GetCommands();
                var maxPad = 16;
                foreach (var cmd in commands.Where(x => x.Pattern.Weight == 1 && x.Pattern.Weight != 0))
                {
                    var name = cmd.Pattern.ToString();

                    var paddingNeeded = maxPad - name.Length;
                    if (paddingNeeded < 0) paddingNeeded = 0;

                    cmdOut.Write(name, ConsoleColor.Green);
                    var padBuilder = new StringBuilder();
                    for (int i = 0; i < paddingNeeded; i++)
                        padBuilder.Append(' ');
                    cmdOut.Write(padBuilder);

                    cmdOut.WriteLine("{0}", ConsoleColor.Cyan, cmd.Description);
                }
            }
            else
            {
                cmdOut.Write("Command: ", ConsoleColor.Green);
                cmdOut.WriteLine(m_CallerInfo.Value.Pattern, ConsoleColor.Cyan);

                cmdOut.Write("Usage: ", ConsoleColor.Green);
                cmdOut.WriteLine("{0} {1}", ConsoleColor.Cyan, m_CallerInfo.Value.Pattern, m_Caller.Syntax);

                cmdOut.Write("Description: ", ConsoleColor.Green);
                cmdOut.WriteLine("{0}", ConsoleColor.Cyan, m_Caller.Description);
            }
        }
    }
}