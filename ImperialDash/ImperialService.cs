using ImperialPlugins;
using ImperialPlugins.Models.Caching;

namespace ImperialDash
{
    public class ImperialService
    {
        public IPCacheClient Cache { get; }
        public ImperialPluginsClient Client { get; }

        public ImperialService()
        {
            Client = new ImperialPluginsClient();
            Cache = new IPCacheClient(Client);
        }
    }
}