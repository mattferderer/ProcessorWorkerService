using ProcessorWorkerService.Services.HttpClients;
using System.Threading.Channels;

namespace ProcessorWorkerService.Services.Producers
{
    public class StreamProducer : ISocialMediaProducer
    {
        private ISocialMediaClient _client;
        private readonly Channel<string> _channel;
        private readonly ILogger _logger;

        public StreamProducer(ISocialMediaClient client, Channel<string> channel, ILogger logger)
        {
            _client = client;
            _channel = channel;
            _logger = logger;
        }

        public async Task ProduceAsync(CancellationToken cancellationToken)
        {
            try
            {
                var latestPostStream = await _client.GetNewContentStream(cancellationToken);
                if (latestPostStream != null)
                {
                    using var streamReader = new StreamReader(latestPostStream);
                    while (!streamReader.EndOfStream && !cancellationToken.IsCancellationRequested)
                    {
                        var line = await streamReader.ReadLineAsync();
                        if (line != null)
                        {
                            await _channel.Writer.WriteAsync(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
            finally
            {
                _logger.LogInformation("Stream Producer Ended");
            }
        }
    }
}