using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models.Attributes;
using ImperialPluginsConsole.Servicing.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ImperialPluginsConsole.Models
{
    public class CommandPattern
    {
        private List<string> m_Pattern;

        private CommandPattern(List<string> pattern)
        {
            m_Pattern = pattern;
            if (pattern.Count > 0 && string.IsNullOrWhiteSpace(pattern[0]))
            {
                m_Pattern = new List<string>();
            }
        }

        public int Weight => m_Pattern.Count;

        public static CommandPattern Create(ICommand command, IActivator activator)
        {
            return new CommandPattern(GetPattern(command, activator));
        }

        public override string ToString()
        {
            return string.Join(" ", m_Pattern);
        }

        public int Matches(IList<string> command)
        {
            if (m_Pattern.Count > command.Count)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < m_Pattern.Count; i++)
                {
                    var src = command[i];
                    var ptn = m_Pattern[i];

                    if (!string.Equals(src, ptn, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return -1;
                    }
                }

                return m_Pattern.Count;
            }
        }

        private static List<string> GetPattern(ICommand cmd, IActivator activator)
        {
            var p = new List<string>();

            p.Insert(0, cmd.Name);

            var cType = cmd.GetType();

            var parentAttr = cType.GetCustomAttribute<CommandParent>();

            if (parentAttr != null && (typeof(ICommand).IsAssignableFrom(parentAttr.ParentType) && parentAttr.ParentType.IsClass))
            {
                var cmd2 = (ICommand)activator.ActivateType(parentAttr.ParentType);

                var SubPattern = GetPattern(cmd2, activator);

                p.InsertRange(0, SubPattern);
            }

            return p;
        }
    }
}