using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Implementations
{
    public class CommandOut : ICommandOut
    {
        public void Write(object message, params object[] args)
        {
            var msg = message.ToString();
            if (msg == null) return;

            Console.Write(string.Format(msg, args: args));
        }

        public void Write(object message, ConsoleColor color, params object[] args)
        {
            var msg = message.ToString();
            if (msg == null) return;

            var prev = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(string.Format(msg, args: args));
            Console.ForegroundColor = prev;
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(object message, params object[] args)
        {
            var msg = message.ToString();
            if (msg == null) return;
            Console.WriteLine(string.Format(msg, args: args));
        }

        public void WriteLine(object message, ConsoleColor color, params object[] args)
        {
            var msg = message.ToString();
            if (msg == null) return;

            var prev = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(string.Format(msg, args: args));
            Console.ForegroundColor = prev;
        }
    }
}