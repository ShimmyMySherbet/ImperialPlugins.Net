using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.Text;

namespace ImperialPluginsConsole.Commands
{
    [CommandParent(typeof(MerchantsCommand))]
    public class MerchantsListCommand : ICommand
    {
        private readonly ImperialPluginsClient m_ImperialPlugins;
        private readonly CommandContext m_Context;

        public string Name => "List";

        public string Syntax => "[-l (limit)]";

        public string Description => "Lists all merchant accounts on the site.";

        public MerchantsListCommand(ImperialPluginsClient imperialPlugins, CommandContext context)
        {
            m_ImperialPlugins = imperialPlugins;
            m_Context = context;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context.ArgumentParser.WithDependants("l").Parse();

            int max = 100;
            if (args.ContainsKey("l") && !int.TryParse(args["l"], out max))
            {
                cmdOut.WriteLine("Max value must be a valid integer.", ConsoleColor.Red);
                return;
            }

            var merchants = m_ImperialPlugins.GetMerchants(max);

            cmdOut.Write("Listing ", ConsoleColor.Green);
            cmdOut.Write($"{merchants.Items.Length}", ConsoleColor.Cyan);
            cmdOut.Write("/", ConsoleColor.Green);
            cmdOut.Write($"{merchants.TotalCount}", ConsoleColor.Cyan);
            cmdOut.WriteLine(" Merchants", ConsoleColor.Green);

            var namePad = 30;
            var emailPad = 40;
            var datePad = 12;

            foreach (var m in merchants.Items)
            {
                cmdOut.Write("[", ConsoleColor.Green);
                cmdOut.Write("{0}", ConsoleColor.Yellow, m.ID);
                cmdOut.Write("] ", ConsoleColor.Green);

                cmdOut.Write("{0}{1}", ConsoleColor.Yellow, m.Name, Pad(m.Name.Length, namePad));

                cmdOut.Write("Joined:", ConsoleColor.Green);
                var crDate = m.CreationTime.ToShortDateString();
                cmdOut.Write("{0}{1}", ConsoleColor.Cyan, crDate, Pad(crDate.Length, datePad));

                if (!string.IsNullOrEmpty(m.SupportEmail))
                {
                    cmdOut.Write("Email:", ConsoleColor.Green);
                    cmdOut.Write("{0}{1}", ConsoleColor.Cyan, m.SupportEmail, Pad(m.SupportEmail.Length, emailPad));
                }
                cmdOut.WriteLine();
                if (!string.IsNullOrEmpty(m.Description))
                {
                    cmdOut.Write("  Description: ", ConsoleColor.Green);
                    cmdOut.WriteLine("{0}", ConsoleColor.Magenta, m.Description);
                }
            }
        }

        private string Pad(int consumed, int padTo)
        {
            if (consumed >= padTo)
                return string.Empty;

            var b = new StringBuilder();
            for (int i = 0; i < padTo - consumed; i++)
                b.Append(' ');
            return b.ToString();
        }
    }
}