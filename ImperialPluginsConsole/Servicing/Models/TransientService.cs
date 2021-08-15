using System;

namespace ImperialPluginsConsole.Servicing.Models
{
    public class TransientService
    {
        public Type SpecifiedType;
        public Type ImplementationType;

        public TransientService(Type specifiedType, Type implementationType)
        {
            SpecifiedType = specifiedType;
            ImplementationType = implementationType;
        }
    }
}