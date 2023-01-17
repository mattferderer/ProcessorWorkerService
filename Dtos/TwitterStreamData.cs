using System.Text.Json.Serialization;

namespace ProcessorWorkerService.Dtos
{
    public class TweetStreamDataDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";
        [JsonPropertyName("edit_history_tweet_ids")]
        public List<string> EditHistoryTweetIds { get; set; } = new List<string>();
    }
}
