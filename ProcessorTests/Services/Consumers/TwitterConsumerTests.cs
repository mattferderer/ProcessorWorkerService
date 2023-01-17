using Microsoft.Extensions.Logging;
using Moq;
using ProcessorWorkerService.Dtos;
using ProcessorWorkerService.Services.Consumers;
using System.Text.Json;
using System.Threading.Channels;

namespace ProcessorTests.Services.Consumers
{
    public class TwitterConsumerTests
    {
        [Fact]
        public async Task TwitterConsumerCanParseTwitterStreamDto()
        {
            // Arrange
            var tweetDto = new TwitterStreamDto
            {
                Data = new TweetStreamDataDto
                {
                    Id = "123",
                    Text = "Hello World"
                }
            };
            var logMock = new Mock<ILogger>();
            var channel = Channel.CreateUnbounded<string>();
            var twitterConsumer = new TwitterConsumer(channel, logMock.Object);
            var tweets = new List<string>();

            // Act
            await channel.Writer.WriteAsync(JsonSerializer.Serialize(tweetDto));
            tweetDto.Data.Text = "Goodbye World";
            await channel.Writer.WriteAsync(JsonSerializer.Serialize(tweetDto));
            channel.Writer.Complete();
            await foreach (string tweet in twitterConsumer.ConsumeAsync(new CancellationToken()))
            {
                tweets.Add(tweet);
            }
            //// Assert
            Assert.Collection<string>(tweets,
                x => Assert.Equal("hello world", x),
                x => Assert.Equal("goodbye world", x));
        }
    }
}
