using System;

namespace ImperialPluginsConsole.Models
{
    public class MissingArgumentException : Exception
    {
        public string Argument { get; init; }

        public MissingArgumentException(string arg)
        {
            Argument = arg;
        }
    }
}