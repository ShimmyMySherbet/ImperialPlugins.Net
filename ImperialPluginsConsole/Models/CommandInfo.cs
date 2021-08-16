using System;

namespace ImperialPluginsConsole.Models
{
    public struct CommandInfo
    {
        public Type CommandType;
        public CommandPattern Pattern;
        public string Syntax;
        public string Dexcription;


        public CommandInfo(Type commandType, CommandPattern patten, string syntax, string dexcription)
        {
            CommandType = commandType;
            Pattern = patten;
            Syntax = syntax;
            Dexcription = dexcription;
        }
    }
}