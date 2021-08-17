using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;

namespace ImperialPluginsConsole.Commands.Customers
{
    public class CustomersCommand : ICommand
    {
        public string Name => "Customers";

        public string Syntax => "";

        public string Description => "Lists customers";

        private ImperialPluginsClient m_ImperialPlugins;

        public CustomersCommand(ImperialPluginsClient imperialPlugins)
        {
            m_ImperialPlugins = imperialPlugins;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var c = m_ImperialPlugins.GetUsers();

            foreach (var m in c.Items)
            {
                cmdOut.WriteLine("{0} {1}", m.Id, m.UserName);
            }
        }
    }
}