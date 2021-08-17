using ImperialPluginsConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Commands.Coupons
{
    public class CouponCommand : ICommand
    {
        public string Name => "Coupons";

        public string Syntax => "";

        public string Description => "Manages Coupons";

        public void Execute(ICommandOut cmdOut)
        {
            throw new NotImplementedException();
        }
    }
}
