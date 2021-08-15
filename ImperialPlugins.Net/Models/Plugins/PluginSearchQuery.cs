using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Plugins
{
    public class PluginSearchQuery
    {
        private Dictionary<string, string> Headers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public PluginSearchQuery WithFilter(EPluginFilter filter, string value)
        {
            Headers[filter.ToString()] = value;
            return this;
        }

        public PluginSearchQuery WithProductIDs(params int[] IDs)
        {
            Headers["ProductIds"] = string.Join(",", IDs);
            return this;
        }
        public PluginSearchQuery WithProductRegistartionIds(params int[] IDs)
        {
            Headers["ProductRegistartionIds"] = string.Join(",", IDs);
            return this;
        }

        public PluginSearchQuery WithBlocked(bool blocked)
        {
            Headers["IsBlocked"] = blocked.ToString();
            return this;
        }


        public PluginSearchQuery WithSkipCount(int skip)
        {
            Headers["SkipCount"] = skip.ToString();
            return this;
        }

        public PluginSearchQuery WithMaxResults(int max)
        {
            Headers["MaxResultCount"] = max.ToString();
            return this;
        }

    }
}
