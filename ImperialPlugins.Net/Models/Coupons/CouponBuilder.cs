using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Coupons
{
    public class CouponBuilder
    {
        public string Name;
        public string Key;
        public float Discount;
        public bool IsEnabled;
        public DateTime? ExpirationTime;
        public DateTime? StartTime;
        public int MaxUsages;
        public bool IsGlobal;
        public bool IsMerchantGlobal;
        public List<int> Products = new List<int>();
    }
}
