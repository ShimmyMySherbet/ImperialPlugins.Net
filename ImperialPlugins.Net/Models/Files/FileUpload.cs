using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Files
{
    public class FileUpload
    {
        public string MarkdownChangelog;
        public bool ForceUpdate;
        public int ProductId;
        public string DisplayVersion;
        public string ProductBranchIdentifier;
        public IPFileData File;
    }
}
