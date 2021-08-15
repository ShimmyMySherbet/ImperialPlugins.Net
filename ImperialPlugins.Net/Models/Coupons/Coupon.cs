using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Coupons
{
    public class Coupon
    {
        public string Name;
        public string Key;
        public float Discount;
        public bool IsEnabled;
        public int[] Products;
        public DateTime ExpirationTime;
        public DateTime? StartTime;
        public int MaxUsages;
        public int Usages;
        public bool IsGlobal;
        public bool IsActive;
        public int ID;
    }
}
