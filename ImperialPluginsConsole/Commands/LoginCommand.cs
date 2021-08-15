using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ImperialPluginsConsole.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly CommandContext m_Context;
        private readonly ImperialPluginsClient m_ImperialPlugins;

        public LoginCommand(CommandContext context, ImperialPluginsClient imperialPlugins)
        {
            m_Context = context;
            m_ImperialPlugins = imperialPlugins;
        }

        public string Name => "Login";

        public string Syntax => "Login [(API Key)] | [(Username) (Password)]";

        public string Description => "Logs into the Imperial Plugins API";

        public void Execute(ICommandOut cmdOut)
        {
            var basePath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            string? authPath = null;
            if (basePath != null)
            {
                authPath = Path.Combine(basePath.FullName, "auth");
            }

            if (m_Context.Arguments.Length == 0)
            {
                // auto login

                if (authPath != null)
                {
                    if (File.Exists(authPath))
                    {
                        var data = File.ReadAllLines(authPath).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (data.Length == 1)
                        {
                            var apiKey = data[0];

                            if (m_ImperialPlugins.CreateLogin().Login(apiKey))
                            {
                                cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                            }
                            else
                            {
                                cmdOut.WriteLine("Login Failed using saved API Key.", ConsoleColor.Red);
                            }
                        }
                        else if (data.Length == 2)
                        {
                            if (m_ImperialPlugins.CreateLogin().Login(data[0], data[1]))
                            {
                                cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                            }
                            else
                            {
                                cmdOut.WriteLine("Login Failed using saved account credentials.", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            cmdOut.Write("Failed to login: ", ConsoleColor.Red);
                            cmdOut.WriteLine("Unknown auth format.");
                        }
                    }
                    else
                    {
                        cmdOut.Write("Failed to login: ", ConsoleColor.Red);
                        cmdOut.WriteLine("No saved credentials");
                    }
                }
                else
                {
                    cmdOut.Write("Failed to login: ", ConsoleColor.Red);
                    cmdOut.WriteLine("Auth cache file path error.");
                }
            }
            else if (m_Context.Arguments.Length == 1)
            {
                var token = m_Context.Arguments[0];
                cmdOut.WriteLine("Logging in...", ConsoleColor.Yellow);

                if (m_ImperialPlugins.CreateLogin().Login(token))
                {
                    cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                    if (authPath != null)
                    {
                        File.WriteAllLines(authPath, new string[] { token });
                    }
                }
                else
                {
                    cmdOut.WriteLine("Login Failed.", ConsoleColor.Red);
                }
            }
            else
            {
                var username = m_Context.Arguments[0];
                var pass = m_Context.Arguments[1];
                cmdOut.WriteLine("Logging in...", ConsoleColor.Yellow);

                if (m_ImperialPlugins.CreateLogin().Login(username, pass))
                {
                    cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                    if (authPath != null)
                    {
                        File.WriteAllLines(authPath, new string[] { username, pass });
                    }
                }
                else
                {
                    cmdOut.WriteLine("Login Failed.", ConsoleColor.Red);
                }
            }
        }
    }
}