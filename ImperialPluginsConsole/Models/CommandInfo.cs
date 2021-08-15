using System;

namespace ImperialPluginsConsole.Models
{
    public struct CommandInfo
    {
        public Type CommandType;
        public string Name;
        public string Syntax;
        public string Dexcription;

        public CommandInfo(Type commandType, string name, string syntax, string dexcription)
        {
            CommandType = commandType;
            Name = name;
            Syntax = syntax;
            Dexcription = dexcription;
        }
    }
}