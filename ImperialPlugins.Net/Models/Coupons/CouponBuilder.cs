using System;
using System.Collections.Generic;

namespace ImperialPlugins.Models.Coupons
{
    public class CouponBuilder
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public float Discount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTime? StartTime { get; set; }
        public int MaxUsages { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsMerchantGlobal { get; set; }
        public List<int> Products { get; set; } = new List<int>();
    }
}
