namespace ImperialPluginsConsole.Interfaces
{
    public interface ICommandReader
    {
        ICommand GetNextCommand();

        void SendCommand(string command);
    }
}