using ImperialPluginsConsole.Models;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Syntax { get; }
        string Description { get; }

        void Execute(ICommandOut cmdOut);
    }
}