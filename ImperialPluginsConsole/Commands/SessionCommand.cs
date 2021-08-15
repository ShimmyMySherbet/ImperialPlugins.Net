using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using System;

namespace ImperialPluginsConsole.Commands
{
    public class SessionCommand : ICommand
    {
        private readonly CommandContext m_Context;
        private readonly ImperialPluginsClient m_ImperialPlugins;

        public string Name => "Session";

        public string Syntax => "[] | [reload]";

        public string Description => "Checks or reloads session info";

        public SessionCommand(CommandContext context, ImperialPluginsClient imperialPlugins)
        {
            m_Context = context;
            m_ImperialPlugins = imperialPlugins;
        }

        public void Execute(ICommandOut cmdOut)
        {
            if (!m_ImperialPlugins.IsLoggedIn)
            {
                cmdOut.Write("Error: ", ConsoleColor.Red);
                cmdOut.Write("Not logged into Imperial Plugins. Use command '", ConsoleColor.Magenta);
                cmdOut.Write("Login", ConsoleColor.Yellow);
                cmdOut.WriteLine("' to login.", ConsoleColor.Magenta);
                return;
            }

            if (m_Context.Arguments.Length > 0)
            {
                var arg = m_Context.Arguments[0];
                if (arg.Equals("reload", StringComparison.InvariantCultureIgnoreCase))
                {
                    cmdOut.WriteLine("Fetching session info...", ConsoleColor.Yellow);
                    try
                    {
                        m_ImperialPlugins.GetSession();

                        cmdOut.WriteLine("Fetched session data.", ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        cmdOut.Write("Failed to load Session: ", ConsoleColor.Red);
                        cmdOut.WriteLine(ex.Message, ConsoleColor.DarkMagenta);
                    }
                }
            }


            cmdOut.Write("Logged in as: ", ConsoleColor.Green);
            cmdOut.Write(m_ImperialPlugins.Session.Merchant.Name, ConsoleColor.Yellow);
            cmdOut.Write(", Email: ", ConsoleColor.Green);
            cmdOut.WriteLine(m_ImperialPlugins.Session.Merchant.SupportEmail, ConsoleColor.Yellow);
        }
    }
}