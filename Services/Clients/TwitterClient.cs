using Microsoft.Extensions.Options;
using ProcessorWorkerService.Configuration.Models;
using System.Net.Http.Headers;

//TODO: Future version add Polly support for re-connecting.
namespace ProcessorWorkerService.Services.HttpClients
{
    public class TwitterClient : ISocialMediaClient
    {
        private HttpClient _client;

        public TwitterClient(IOptions<TwitterApiConfig> config)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.twitter.com");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Value.BearerToken);
        }

        public async Task<Stream?> GetNewContentStream(CancellationToken cancellationToken) => await _client.GetStreamAsync("/2/tweets/sample/stream", cancellationToken);
    }
}
