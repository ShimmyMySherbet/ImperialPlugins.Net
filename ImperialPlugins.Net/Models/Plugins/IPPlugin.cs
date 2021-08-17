using ImperialPlugins.Models.Assets;
using ImperialPlugins.Models.Users;
using System;
using System.Collections.Generic;

namespace ImperialPlugins.Models.Plugins
{
    public class IPPlugin
    {
        public int ID;
        public string Name;
        public string NameID;
        public string ShortDescription;
        public string LicensingMode;
        public decimal UnitPrice;
        public bool IsPublic;
        public float? FeaturesRating;
        public float? EasinessRating;
        public float? SupportRating;
        public float? AverageRating;
        public int RatingCount;
        public bool IsCurated;
        public DateTime? LastUpdateTime;
        public string Status;
        public DateTime? PublishTime;
        public DateTime? PublishRequestTime;
        public DateTime CreationTime;
        public string CreatorId;
        public Merchant Merchant;
        public int? ProductTermsId;
        public PluginCategory GameCategory;
        public List<LogoFile> LogoFiles;
        public List<PluginCategory> Categories;
    }
}