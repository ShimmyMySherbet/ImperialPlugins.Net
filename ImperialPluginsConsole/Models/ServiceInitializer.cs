using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Servicing.Interfaces;

namespace ImperialPluginsConsole.Models
{
    public class ServiceInitializer : IHostedService
    {
        private readonly ICommandService m_CommandService;

        public ServiceInitializer(ICommandService commandService)
        {
            m_CommandService = commandService;
        }

        public void Start()
        {
            m_CommandService.Mount();
        }

        public void Stop()
        {
        }
    }
}