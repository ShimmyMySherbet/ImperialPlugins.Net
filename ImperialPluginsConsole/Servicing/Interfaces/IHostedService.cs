using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Servicing.Interfaces
{
    public interface IHostedService
    {
        void Start();
        void Stop();
    }
}
