using System;

namespace ImperialPluginsConsole.Interfaces
{
    public interface ICommandOut
    {
        void Write(object message, params object[] args);

        void Write(object message, ConsoleColor color, params object[] args);

        void WriteLine();

        void WriteLine(object message, params object[] args);

        void WriteLine(object message, ConsoleColor color, params object[] vs);
    }
}