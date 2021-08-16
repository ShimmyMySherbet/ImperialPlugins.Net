using System;

namespace ImperialPluginsConsole.Models
{
    public struct CommandInfo
    {
        public Type CommandType;
        public CommandPattern Pattern;
        public string Syntax;
        public string Description;


        public CommandInfo(Type commandType, CommandPattern patten, string syntax, string description)
        {
            CommandType = commandType;
            Pattern = patten;
            Syntax = syntax;
            Description = description;
        }
    }
}