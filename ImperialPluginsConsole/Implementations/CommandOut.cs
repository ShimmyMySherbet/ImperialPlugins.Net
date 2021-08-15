using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Implementations
{
    public class CommandOut : ICommandOut
    {
        public void Write(string message, params object[] args)
        {
            Console.Write(string.Format(message, args: args));
        }

        public void Write(string message, ConsoleColor color, params object[] args)
        {
            var prev = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(string.Format(message, args: args));
            Console.ForegroundColor = prev;
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string message, params object[] args)
        {
            Console.WriteLine(string.Format(message, args: args));
        }

        public void WriteLine(string message, ConsoleColor color, params object[] args)
        {
            var prev = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(string.Format(message, args: args));
            Console.ForegroundColor = prev;
        }
    }
}