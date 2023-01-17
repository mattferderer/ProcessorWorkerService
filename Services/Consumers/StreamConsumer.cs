using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace ProcessorWorkerService.Services.Consumers
{
    public class StreamConsumer : ISocialMediaConsumer
    {
        private readonly Channel<string> _channel;
        private readonly ILogger _logger;

        public StreamConsumer(Channel<string> channel, ILogger logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async IAsyncEnumerable<string> ConsumeAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (await _channel.Reader.WaitToReadAsync() && !cancellationToken.IsCancellationRequested)
            {
                while (_channel.Reader.TryRead(out string? channelMsg))
                {
                    yield return ParseString(channelMsg);
                }
            }
            _logger.LogInformation("Finished Consuming Process");
        }

        public virtual string ParseString(string msg) => msg;
    }
}
