using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands.Products
{
    public class ProductCommand : ICommand
    {
        public string Name => "Product";

        public string Syntax => "";

        public string Description => "Manages products";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("Block, Unblock", ConsoleColor.Yellow);
        }
    }
}