using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Service
{
    public class Ticket : IPObject
    {
        public int TicketId;
        public string MarkdownContent;
        public string HtmlContent;
        public string CreatorUserName;
        public List<object> Files;
        public bool IsDeleted;
        public object DeleterId;
        public object DeletionTime;
        public object LastModificationTime;
        public object LastModifierId;
        public DateTime CreationTime;
        public string CreatorId;
        public int Id;

        [JsonIgnore] public ImperialPluginsClient ImperialPlugins { get; set; }
    }
}
