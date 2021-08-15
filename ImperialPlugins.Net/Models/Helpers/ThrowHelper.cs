using ImperialPlugins.Models.Exceptions;

namespace ImperialPlugins.Models.Helpers
{
    public class ThrowHelper
    {
        private ImperialPluginsClient Client;

        public ThrowHelper(ImperialPluginsClient client)
        {
            Client = client;
        }

        public void ThrowIfNotLoggedIn()
        {
            if (!Client.IsLoggedIn)
            {
                throw new NotLoggedInException();
            }
        }
    }
}