using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Servicing.Interfaces;
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