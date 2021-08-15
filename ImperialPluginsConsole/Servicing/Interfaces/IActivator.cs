using System;

namespace ImperialPluginsConsole.Servicing.Interfaces
{
    public interface IActivator
    {
        T ActivateType<T>(params object[] parameters);
        object ActivateType(Type t, params object[] parameters);
    }
}