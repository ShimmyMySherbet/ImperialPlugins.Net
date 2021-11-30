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
            var args = m_Context.ArgumentParser
                .WithDependants("l")
                .Parse();

            var max = args.GetOrDefault("l", 100);

            var users = m_Cache.GetUsers(max);

            var userPadTo = users.Items.GetPadBase(x => x.UserName.Length + (x.Email ?? "").Length) + 2;

            var createdDates = new string[users.Items.Length];

            for (int i = 0; i < users.Items.Length; i++)
            {
                var I = users.Items[i];
                createdDates[i] = I.CreationTime.ToShortDateString();
            }

            var datePadTo = createdDates.GetPadBase(x => x.Length) + 2;

            cmdOut.WriteLine();

            for (int i = 0; i < users.Items.Length; i++)
            {
                var usr = users.Items[i];

                cmdOut.Write("[", System.ConsoleColor.Green);
                cmdOut.Write(usr.Id, System.ConsoleColor.Yellow);
                cmdOut.Write("] ", System.ConsoleColor.Green);
                cmdOut.Write("User: ", System.ConsoleColor.Blue);
                cmdOut.Write(usr.UserName);
                cmdOut.Write(" [", System.ConsoleColor.Green);
                cmdOut.Write(usr.Email, System.ConsoleColor.DarkCyan);
                cmdOut.Write("]".Pad(userPadTo, usr.UserName.Length + usr.Email.Length), System.ConsoleColor.Green);

                cmdOut.Write("Created: ", System.ConsoleColor.Blue);
                cmdOut.WriteLine(createdDates[i].Pad(datePadTo));
            }
        }
    }
}