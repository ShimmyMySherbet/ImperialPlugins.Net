using System;

namespace ImperialPlugins.Models.Coupons
{
    public class Coupon
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public float Discount { get; set; }
        public bool IsEnabled { get; set; }
        public int[] Products { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTime? StartTime { get; set; }
        public int MaxUsages { get; set; }
        public int Usages { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsActive { get; set; }
        public int ID { get; set; }
    }
}
