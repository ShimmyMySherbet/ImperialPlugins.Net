using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;

namespace ImperialPluginsConsole.Commands.Customers
{
    [CommandParent(typeof(CustomersCommand))]
    public class CustomerListCommand : ICommand
    {
        public string Name => "List";

        public string Syntax => "[-l limit]";

        public string Description => "Lists customers of your products";

        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;
        private readonly CacheClient m_Cache;

        public CustomerListCommand(ImperialPluginsClient imperialPlugins, CommandContext context, CacheClient cache)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {






        }
    }
}