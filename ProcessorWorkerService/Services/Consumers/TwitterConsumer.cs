using ProcessorWorkerService.Dtos;
using System.Text.Json;
using System.Threading.Channels;

namespace ProcessorWorkerService.Services.Consumers
{
    public class TwitterConsumer : StreamConsumer
    {
        public TwitterConsumer(Channel<string> channel, ILogger logger) : base(channel, logger)
        {
        }

        public override string ParseString(string msg) =>
            JsonSerializer.Deserialize<TwitterStreamDto>(msg)?.Data.Text.ToLower() ?? "";
    }
}
