using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Reviews
{
    public class MerchantResponse 
    {
        public DateTime CreationTime;
        public string CreatorId;
        public DateTime LastModificationTime;
        public string LastModifierId;
        public int ProductReviewId;
        public string HtmlContent;
        public string MarkdownContent;
    }
}
