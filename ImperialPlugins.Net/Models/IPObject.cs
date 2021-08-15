using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models
{
    public interface IPObject
    {
        ImperialPluginsClient ImperialPlugins { get; set; }
    }
}
