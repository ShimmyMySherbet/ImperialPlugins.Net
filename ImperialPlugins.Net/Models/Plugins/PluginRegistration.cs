using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Plugins
{
    public class PluginRegistration : IPObject
    {
        public string OwnerID;
        public string OwnerName;

        public string LicenseKey;
        public int ProductID;
        public DateTime? ExpireTime;
        public DateTime? RefundTime;
        public bool IsBlocked;
        public string BlockDisplayReason;
        public bool IsActive;
        public string MerchantID;
        public DateTime CreationTime;
        public int ID;

        [JsonIgnore]
        public bool SpecialLicence => !IsActive && !IsBlocked && RefundTime == null;

        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins { get; set; }
        public RefundVerifyToken Refund() => ImperialPlugins.RefundProduct(LicenseKey);
        public void BlockLicence(string reason) => ImperialPlugins.BlockProduct(LicenseKey, reason);
        public void UnblockLicence() => ImperialPlugins.UnblockProduct(LicenseKey);
    }
}
