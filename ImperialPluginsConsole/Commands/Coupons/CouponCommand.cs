using System;
using ImperialPluginsConsole.Interfaces;

namespace ImperialPluginsConsole.Commands.Coupons
{

    public class CouponCommand : ICommand
    {
        public string Name => "Coupons";

        public string Syntax => "";

        public string Description => "Manages Coupons";

        public void Execute(ICommandOut cmdOut)
        {
            cmdOut.Write("Available commands: ", ConsoleColor.Green);
            cmdOut.WriteLine("List, Create, Delete", ConsoleColor.Yellow);
        }
    }
}
