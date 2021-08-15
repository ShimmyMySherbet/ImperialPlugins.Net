using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Plugins
{
    public class PluginCategory
    {
        public string CategoryName;
        public string ShortDescription;
        public string IconUrl;
        public int ParentCategoryId;
        public int ID;

        [JsonIgnore]
        public string RealIconURL => $"https://imperialplugins.com/{IconUrl}";
    }
}
