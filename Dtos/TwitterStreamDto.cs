using System.Text.Json.Serialization;

namespace ProcessorWorkerService.Dtos
{
    public class TwitterStreamDto
    {
        [JsonPropertyName("data")]
        public TweetStreamDataDto Data { get; set; } = new TweetStreamDataDto();
    }
}
