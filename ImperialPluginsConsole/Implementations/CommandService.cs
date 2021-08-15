using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using ImperialPluginsConsole.Servicing.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ImperialPluginsConsole.Implementations
{
    public class CommandService : ICommandService
    {
        private readonly IServiceContainer m_Container;
        private readonly IActivator m_Activator;

        private IDictionary<string, CommandInfo> m_Commands = new Dictionary<string, CommandInfo>(StringComparer.InvariantCultureIgnoreCase);
        private CommandInfo? m_DefaultCommand;


        public CommandService(IServiceContainer container, IActivator activator)
        {
            m_Container = container;
            m_Activator = activator;
        }

        public CommandInfo[] GetCommands()
        {
            return m_Commands.Values.Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();
        }

        public void Mount()
        {
            foreach(var cmdType in Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof(ICommand).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract))
            {

                var blankContext = new CommandContext(string.Empty, new string[0]);

                var instance = (ICommand)m_Activator.ActivateType(cmdType, blankContext);

                var info = new CommandInfo(cmdType, instance.Name, instance.Syntax, instance.Description);

                m_Commands[info.Name] = info;


                if (Attribute.IsDefined(cmdType, typeof(DefaultCommand)))
                {
                    m_DefaultCommand = info;
                }

            }
        }

        public ICommand? Parse(string input)
        {
            var parameters = (from Match m in Regex.Matches(input.Trim(' '), "[\\\"](.+?)[\\\"]|([^ ]+)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled)
                              select m.Value.Trim('"').Trim()).ToList();

            if (parameters.Count == 0) return null;

            var cmdName = parameters[0].Trim();
            parameters.RemoveAt(0);


            if (string.IsNullOrEmpty(cmdName)) return null;

            if (m_Commands.ContainsKey(cmdName))
            {
                var context = new CommandContext(cmdName, parameters.ToArray());

                var instance = m_Activator.ActivateType(m_Commands[cmdName].CommandType, context);

                return (ICommand)instance;
            }

            
            if (m_DefaultCommand != null)
            {
                var fallbackContext = new CommandContext(cmdName, parameters.ToArray(), true);
                return (ICommand)m_Activator.ActivateType(m_DefaultCommand.Value.CommandType, fallbackContext);
            }

            return null;
        }
    }
}