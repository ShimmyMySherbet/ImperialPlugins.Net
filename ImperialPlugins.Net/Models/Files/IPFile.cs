using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Files
{
    public class IPFile
    {
        public int ID;
        public int ProductID;
        public string FileName;
        public string Version;
        public string DisplayVersion;
        public string ProductPlatformNameId;
        public string ProductBranchId;
        public DateTime? CreationTime;
    }
}
