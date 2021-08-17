using ImperialPluginsConsole.Interfaces;

namespace ImperialPluginsConsole.Commands.Products
{
    public class ProductsCommand : ICommand
    {
        public string Name => "Products";

        public string Syntax => "";

        public string Description => "Manages store products";

        public void Execute(ICommandOut cmdOut)
        {
        }
    }
}