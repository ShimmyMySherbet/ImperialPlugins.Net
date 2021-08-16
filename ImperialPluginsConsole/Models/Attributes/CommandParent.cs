using System;

namespace ImperialPluginsConsole.Models.Attributes
{
    public class CommandParent : Attribute
    {
        public Type ParentType { get; init; }

        public CommandParent(Type parent)
        {
            ParentType = parent;
        }
    }
}