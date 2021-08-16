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

        private List<CommandInfo> m_Commands = new List<CommandInfo>();
        private CommandInfo? m_DefaultCommand;

        public CommandService(IServiceContainer container, IActivator activator)
        {
            m_Container = container;
            m_Activator = activator;
        }

        public CommandInfo[] GetCommands()
        {
            return m_Commands.ToArray();
        }

        public void Mount()
        {
            foreach (var cmdType in Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof(ICommand).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract))
            {
                var blankContext = new CommandContext(string.Empty, new string[0]);

                var instance = (ICommand)m_Activator.ActivateType(cmdType, blankContext);

                var pattern = CommandPattern.Create(instance, m_Activator);

                var info = new CommandInfo(cmdType, pattern, instance.Syntax, instance.Description);

                m_Commands.Add(info);

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

            var map = new List<WeightInfoPair>();

            foreach (var m in m_Commands)
            {
                var pair = new WeightInfoPair(m.Pattern.Matches(parameters), m);
                map.Add(pair);
            }

            if (map.Count != 0)
            {
                var best = map.OrderByDescending(x => x.Weight).ElementAt(0);
                if (best.Weight > 0)
                {
                    var remParam = parameters.Skip(best.Weight);

                    var context = new CommandContext(best.Info.Pattern.ToString(), remParam.ToArray());

                    return (ICommand)m_Activator.ActivateType(best.Info.CommandType, context);
                }
            }

            if (m_DefaultCommand != null)
            {
                var fallbackContext = new CommandContext("", parameters.ToArray(), true);
                return (ICommand)m_Activator.ActivateType(m_DefaultCommand.Value.CommandType, fallbackContext);
            }

            return null;
        }
    }
}