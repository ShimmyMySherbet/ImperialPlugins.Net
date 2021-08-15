using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Servicing.Interfaces;
using System;

namespace ImperialPluginsConsole.Servicing.Models
{
    public class HostedService
    {
        public Type ServiceType;
        public IHostedService Instance;

        public HostedService(Type serviceType, IHostedService instance)
        {
            ServiceType = serviceType;
            Instance = instance;
        }
    }
}