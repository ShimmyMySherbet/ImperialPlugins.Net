using System;

namespace ImperialPluginsConsole.Interfaces
{
    public interface ICommandOut
    {
        void Write(string message, params object[] args);

        void Write(string message, ConsoleColor color, params object[] args);

        void WriteLine();

        void WriteLine(string message, params object[] args);

        void WriteLine(string message, ConsoleColor color, params object[] vs);
    }
}