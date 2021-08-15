using System;

namespace ImperialPluginsConsole.Servicing.Models
{
    public class SingletonService
    {
        public Type Type;
        public Type SpecifiedType;
        public object Instance;

        public SingletonService(Type implementationType, Type specified, object instance)
        {
            Type = implementationType;
            SpecifiedType = specified;
            Instance = instance;
        }

        public bool Matches(Type target)
        {
            return target.IsAssignableFrom(Type);
        }
    }
}