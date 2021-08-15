using System.Net;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Plugins
{
    public struct RefundVerifyToken
    {
        private ImperialPluginsClient Client;
        public string LicenceKey { get; private set; }

        public RefundVerifyToken(ImperialPluginsClient client, string key)
        {
            Client = client;
            LicenceKey = key;
        }

        public void Abort()
        {
            LicenceKey = null;
            Client = null;
        }

        /// <summary>
        /// Confirms the refunding of a product.
        /// WARNING: This cannot be undone
        /// </summary>
        public void ConfirmRefund()
        {
            if (LicenceKey == null)
            {
                throw new TaskCanceledException();
            }
            Client.BasicAPIOperation($"/Products/Registrations/Refund?licenseKey={WebUtility.UrlEncode(LicenceKey)}", "POST", new object());
        }
    }
}