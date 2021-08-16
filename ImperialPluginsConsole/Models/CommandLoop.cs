using ImperialPlugins.Models.Exceptions;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Servicing.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Models
{
    public class CommandLoop : IHostedService
    {
        private readonly ICommandReader m_Reader;
        private readonly ICommandOut m_Out;

        public CommandLoop(ICommandReader reader, ICommandOut cout)
        {
            m_Reader = reader;
            m_Out = cout;
        }

        public void Start()
        {
            while (true)
            {
                var cmd = m_Reader.GetNextCommand();

                try
                {
                    cmd.Execute(m_Out);
                }
                catch(NotLoggedInException)
                {
                    m_Out.WriteLine("Error: You need to be logged in to use this command", ConsoleColor.Red);
                }
                catch (MissingArgumentException m)
                {
                    m_Out.Write("Missing required argument: ", ConsoleColor.Red);
                    m_Out.WriteLine($"-{m.Argument}", ConsoleColor.Yellow);
                }
                catch (ImperialPluginsException ipError)
                {
                    m_Out.Write("Error running command: ", ConsoleColor.Red);
                    m_Out.WriteLine("{0}", ConsoleColor.Yellow, ipError.Message);
                    if (ipError.Errors != null)
                    {
                        foreach (var e in ipError.Errors)
                        {
                            m_Out.WriteLine("{0}", ConsoleColor.Yellow, e);
                        }
                    }
                }
                catch (WebException webex)
                {
                    m_Out.Write("Failiure running command: ", ConsoleColor.Red);
                    m_Out.WriteLine("{0}", ConsoleColor.Yellow, webex.Message);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        public void Stop()
        {
        }
    }
}