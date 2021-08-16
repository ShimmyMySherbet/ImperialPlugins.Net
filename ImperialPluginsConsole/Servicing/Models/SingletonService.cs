using System;

namespace ImperialPluginsConsole.Servicing.Models
{
    public class SingletonService
    {
        public Type ImplementationType;
        public Type SpecifiedType;
        public object Instance;

        public SingletonService(Type implementationType, Type specified, object instance)
        {
            ImplementationType = implementationType;
            SpecifiedType = specified;
            Instance = instance;
        }

        public bool Matches(Type target)
        {
            return target.IsAssignableFrom(ImplementationType);
        }
    }
}