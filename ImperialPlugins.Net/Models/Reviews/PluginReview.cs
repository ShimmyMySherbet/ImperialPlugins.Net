using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Reviews
{
    public class PluginReview
    {
        public int Id;
        public DateTime CreationTime;
        public string CreatorId;
        public DateTime LastModificationTime;
        public string LastModifierId;
        public string Title;
        public int ProductId;
        public int ProductFileId;
        public string Content;
        public int FeaturesRating;
        public int SupportRating;
        public int EasinessRating;
        public bool IsHidden;
        public string CreatorName;
        public int TotalFeedbackCount;
        public int HelpfulFeedbackCount;
        public bool IsHelpfulForCurrentUser;
        public ReviewResponse Response;


        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins { get; set; }

        public void Respond(string response) => ImperialPlugins.RespondToReview(Id.ToString(), response);



    }
}
