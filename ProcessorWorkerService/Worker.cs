using ProcessorWorkerService.Services.Analyzers;
using ProcessorWorkerService.Services.Consumers;
using ProcessorWorkerService.Services.DataStores;
using ProcessorWorkerService.Services.Producers;

namespace ProcessorWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISocialMediaProducer _producer;
        private readonly ISocialMediaConsumer _consumer;
        private readonly ISocialMediaStore _dataStore;
        private int _currentPostCount = 0;

        public Worker(ILogger<Worker> logger, ISocialMediaProducer producer, ISocialMediaConsumer consumer, ISocialMediaStore simpleMemoryStore)
        {
            _logger = logger;
            _producer = producer;
            _consumer = consumer;
            _dataStore = simpleMemoryStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await StartTweetHashtagCollection();
            }
        }

        public async Task StartTweetHashtagCollection()
        {
            var cancelTweetProcessing = new CancellationTokenSource();
            ProcessTweets(cancelTweetProcessing.Token);

            await Task.Delay(5000);
            await LogStatsToConsole(cancelTweetProcessing.Token);

            // If we get here, re-start as we most likely couldn't keep up.
            // Cancel current processing. Wait & restart this method.
            cancelTweetProcessing.Cancel();
            await Task.Delay(3000);
            await StartTweetHashtagCollection();
        }

        private async Task LogStatsToConsole(CancellationToken stoppingToken)
        {
            while (_dataStore.GetPostCount() > _currentPostCount)
            {
                _currentPostCount = _dataStore.GetPostCount();
                Console.WriteLine($"Tweet Total {_currentPostCount}");
                Console.WriteLine($"Top 10 Hashtags: {string.Join(", ", _dataStore.GetTopHashTags(10))}");
                await Task.Delay(5000);
            }
        }

        private void ProcessTweets(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () => await _producer.ProduceAsync(stoppingToken));

            _ = Task.Run(async () =>
            {
                await foreach (string msg in _consumer.ConsumeAsync(stoppingToken))
                {
                    _dataStore.IncrementPostCount();
                    _dataStore.AddHashTags(HashtagAnalyzer.GetHashTags(msg));
                }
                return Task.CompletedTask;
            }, stoppingToken);
        }
    }
}