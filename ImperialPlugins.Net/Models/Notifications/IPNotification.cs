using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Notifications
{
    public class IPNotification : IPObject
    {
        public string Title;
        public string HtmlContent;
        public string MarkdownContent;
        public string ThumbnailUrl;
        public string Type;
        public DateTime creationTime;
        public string Url;
        public DateTime? readTime;
        public string ID;

        [JsonIgnore] public ImperialPluginsClient ImperialPlugins { get; set; }

        [JsonIgnore]
        public ENotificationType NotificationType => ImperialPlugins.NotificationTypeParseClient.GetNotificationType(this);
    }
}
