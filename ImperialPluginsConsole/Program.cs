using ImperialPlugins;
using ImperialPluginsConsole.Implementations;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Servicing;
using System.Linq;

namespace ImperialPluginsConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHost();

            host.RegisterSingleton<ICommandService, CommandService>();
            host.RegisterSingleton<ICommandReader, ConsoleCommandReader>();
            host.RegisterSingleton<ImperialPluginsClient>();

            host.RegisterTransient<ICommandOut, CommandOut>();

            host.RegisterHostedService<ServiceInitializer>();
            host.RegisterHostedService<CommandLoop>();

            RegisterAutoCommands(host, args);

            host.Run();
        }

        private static void RegisterAutoCommands(ServiceHost host, string[] args)
        {
            var autoCommands = args.Where(x => x.StartsWith("+")).Select(x => x.Substring(1));
            var provider = host.Resolve<ICommandReader>();

            foreach (var cmd in autoCommands)
            {
                provider.SendCommand(cmd);
            }
        }
    }
}