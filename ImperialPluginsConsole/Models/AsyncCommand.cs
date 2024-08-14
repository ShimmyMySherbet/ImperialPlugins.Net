using System.Threading.Tasks;
using ImperialPluginsConsole.Interfaces;

namespace ImperialPluginsConsole.Models
{
    public abstract class AsyncCommand : ICommand
    {
        public abstract string Name { get; }
        public abstract string Syntax { get; }
        public abstract string Description { get; }

        public void Execute(ICommandOut cmdOut)
        {
            var task = ExecuteAsync(cmdOut);
            task.Wait(-1);
        }

        public abstract Task ExecuteAsync(ICommandOut cmdOut);
    }
}
