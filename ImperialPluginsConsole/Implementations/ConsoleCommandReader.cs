using ImperialPluginsConsole.Interfaces;
using System;
using System.Collections.Generic;

namespace ImperialPluginsConsole.Implementations
{
    public class ConsoleCommandReader : ICommandReader
    {
        private readonly ICommandService m_Commands;
        private Queue<string> m_CommandQueue = new Queue<string>();
        private readonly ICommandOut m_Out;

        public ConsoleCommandReader(ICommandService commands, ICommandOut commandOut)
        {
            m_Commands = commands;
            m_Out = commandOut;
        }

        public ICommand GetNextCommand()
        {
            while (true)
            {
                if (m_CommandQueue.TryDequeue(out var queuedCmd))
                {
                    var instance = m_Commands.Parse(queuedCmd);

                    if (instance == null)
                        continue;

                    return instance;

                }

                m_Out.Write(" > ", ConsoleColor.Green);
                var ln = Console.ReadLine();

                if (ln == null)
                {
                    continue;
                }

                var cmds = ln.Split('&');

                for (int i = 0; i < cmds.Length; i++)
                {
                    var cmd = cmds[i];

                    if (!string.IsNullOrWhiteSpace(cmd))
                    {
                        m_CommandQueue.Enqueue(cmd);
                    }
                }
            }
        }

        public void SendCommand(string command)
        {
             m_CommandQueue.Enqueue(command);
        }
    }
}