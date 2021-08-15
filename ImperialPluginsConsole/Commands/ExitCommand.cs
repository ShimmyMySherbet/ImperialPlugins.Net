using ImperialPluginsConsole.Interfaces;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands
{
    public class ExitCommand : ICommand
    {
        public string Name => "Exit";

        public string Syntax => "";

        public string Description => "Closes the application.";

        public void Execute(ICommandOut cmdOut)
        {
            throw new TaskCanceledException();
        }
    }
}