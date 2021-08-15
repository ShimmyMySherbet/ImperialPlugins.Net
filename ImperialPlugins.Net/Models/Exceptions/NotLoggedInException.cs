using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Exceptions
{
    public sealed class NotLoggedInException : Exception
    {
        public override string Message => "The client is not logged in. Log in using .CreateLogin().Login(...)";
    }
}
