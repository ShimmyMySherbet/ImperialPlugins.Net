using ImperialPluginsConsole.Interfaces;
using System;

namespace ImperialPluginsConsole.Commands.Products
{
    public class ProductsCommand : ICommand
    {
        public string Name => "Products";

        public string Syntax => "";

        public string Description => "Manages store products";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available Commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("Update, List", ConsoleColor.Cyan);
        }
    }
}