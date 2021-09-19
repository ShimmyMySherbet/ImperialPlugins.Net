using ImperialPlugins;
using ImperialPlugins.Models;
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
        private readonly CacheClient m_CacheClient;

        public LoginCommand(CommandContext context, ImperialPluginsClient imperialPlugins, CacheClient cacheClient)
        {
            m_Context = context;
            m_ImperialPlugins = imperialPlugins;
            m_CacheClient = cacheClient;
        }

        public string Name => "Login";

        public string Syntax => "Login [(API Key)] | [(Username) (Password)] | [-h (.HAR file path)]";

        public string Description => "Logs into the Imperial Plugins API\nIf no parameters are supplied, saved credentials are used instead.";

        public void Execute(ICommandOut cmdOut)
        {
            var cArgs = m_Context.ArgumentParser.WithDependants("h").Parse();

            var basePath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            string? authPath = null;
            if (basePath != null)
            {
                authPath = Path.Combine(basePath.FullName, "auth");
            }

            if (cArgs.ContainsKey("h"))
            {
                var fileName = cArgs["h"];

                if (!File.Exists(fileName))
                {
                    cmdOut.Write("Failed to login: HAR file does not exist", ConsoleColor.Red);
                    return;
                }

                if (m_ImperialPlugins.CreateLogin().HarLogin(fileName))
                {
                    cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                    m_CacheClient.StartInit();

                    if (authPath != null)
                    {
                        File.WriteAllLines(authPath, new string[] { "-RAW", m_ImperialPlugins.SessionCredentials.Header, m_ImperialPlugins.SessionCredentials.AuthHeaderContent });
                    }
                    return;
                }
                else
                {
                    cmdOut.WriteLine("Login Failed using extraced credentials.", ConsoleColor.Red);
                    return;
                }
            }

            if (m_Context.Args.Length == 0)
            {
                // auto login

                if (authPath != null)
                {
                    if (File.Exists(authPath))
                    {
                        var data = File.ReadAllLines(authPath).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (data.Length == 1)
                        {
                            cmdOut.WriteLine("Logging in...", ConsoleColor.Green);

                            var apiKey = data[0];

                            if (m_ImperialPlugins.CreateLogin().Login(apiKey))
                            {
                                cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                                m_CacheClient.StartInit();
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
                        else if (data.Length == 3)
                        {
                            var rawheader = data[2];
                            var cred = new IPSessionCredentials() { Header = data[1], AuthHeaderContent = data[2] };

                            if (m_ImperialPlugins.CreateLogin().Login(cred))
                            {
                                cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                            }
                            else
                            {
                                cmdOut.WriteLine("Login Failed using saved credentials.", ConsoleColor.Red);
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
            else if (m_Context.Args.Length == 1)
            {
                var token = m_Context.Args[0];
                cmdOut.WriteLine("Logging in...", ConsoleColor.Yellow);

                if (m_ImperialPlugins.CreateLogin().Login(token))
                {
                    cmdOut.WriteLine("Logged in.", ConsoleColor.Green);
                    if (authPath != null)
                    {
                        File.WriteAllLines(authPath, new string[] { token });
                    }
                    m_CacheClient.StartInit();
                }
                else
                {
                    cmdOut.WriteLine("Login Failed.", ConsoleColor.Red);
                }
            }
            else
            {
                var username = m_Context.Args[0];
                var pass = m_Context.Args[1];
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