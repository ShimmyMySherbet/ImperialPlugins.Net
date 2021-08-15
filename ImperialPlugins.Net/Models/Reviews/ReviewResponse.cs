using System;
using Newtonsoft.Json;

namespace ImperialPlugins.Models.Reviews
{
    public class ReviewResponse : IPObject
    {
        public DateTime CreationTime;
        public string CreatorId;
        public DateTime LastModificationTime;
        public string LastModifierId;
        public int ProductReviewId;
        public string HtmlContent;
        public string MarkdownContent;

        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins { get; set; }

        public void UpdateResponse(string response) => ImperialPlugins.RespondToReview(ProductReviewId.ToString(), response);


    }
}