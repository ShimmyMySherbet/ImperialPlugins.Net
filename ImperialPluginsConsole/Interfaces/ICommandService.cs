using ImperialPluginsConsole.Models;

namespace ImperialPluginsConsole.Interfaces
{
    public interface ICommandService
    {
        void Mount();

        ICommand? Parse(string input);

        CommandInfo[] GetCommands();
    }
}