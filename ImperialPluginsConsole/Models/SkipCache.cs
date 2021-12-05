using System;
using System.Collections.Generic;
using System.Linq;
using ImperialPlugins;
using ImperialPlugins.Models;
using ImperialPlugins.Models.Plugins;

namespace ImperialPluginsConsole.Models
{
    public class SkipCache : CacheClient
    {
        public SkipCache(ImperialPluginsClient imperialPlugins) : base(imperialPlugins)
        {
        }

        public override List<IPPlugin> GetMerchantPlugins(string merchantID)
        {
            return m_ImperialPlugins.GetPlugins(1000).Items.Where(x => x.Merchant.ID == merchantID).ToList();
        }

        public override IPPlugin? GetPlugin(int id)
        {
            return m_ImperialPlugins.GetPlugins(1000).Items.FirstOrDefault(x => x.ID == id);
        }

        public override IPPlugin? GetPluginByName(string name)
        {
            return m_ImperialPlugins.GetPlugins(1000).Items.FirstOrDefault(x => x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override EnumerableResponse<IPPlugin> GetPlugins(int max = 20)
        {
            return m_ImperialPlugins.GetPlugins(max);
        }

        public override PluginRegistration? GetRegistration(int ID)
        {
            return base.GetRegistration(ID);
        }

        public override EnumerableResponse<PluginRegistration> GetRegistrations(int max = 20, bool refresh = false)
        {
            return m_ImperialPlugins.GetRegistrations(max);
        }

        public override List<IPPlugin> GetSelfPlugins()
        {
            return base.GetSelfPlugins();
        }

        public override IPUser? GetUser(string userHandle)
        {
            var users = m_ImperialPlugins.GetUsers(100000);
            return users.Items.FirstOrDefault(x => x.Id == userHandle || x.Email.Equals(userHandle, StringComparison.InvariantCultureIgnoreCase) || x.UserName.Equals(userHandle, StringComparison.InvariantCultureIgnoreCase));
        }

        public override EnumerableResponse<IPUser> GetUsers(int max = 20)
        {
            return m_ImperialPlugins.GetUsers(max);
        }

        public override void RefreshReg()
        {
        }

        public override void StartInit()
        {
        }
    }
}