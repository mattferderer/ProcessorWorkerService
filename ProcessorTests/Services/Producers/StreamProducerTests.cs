using Microsoft.Extensions.Logging;
using Moq;
using ProcessorWorkerService.Services.HttpClients;
using ProcessorWorkerService.Services.Producers;
using System.Text;
using System.Threading.Channels;

namespace ProcessorTests.Services.Producers
{
    public class StreamProducerTests
    {

        [Fact]
        public async Task StreamProducerCanWriteToChannelFromClient()
        {
            {
                // Arrange
                var logMock = new Mock<ILogger>();
                var channel = Channel.CreateUnbounded<string>();
                var clientMock = new Mock<ISocialMediaClient>();
                var clientStream = new MemoryStream(Encoding.UTF8.GetBytes($"Hello World{Environment.NewLine}Goodbye World"));
                clientMock.Setup(x => x.GetNewContentStream(It.IsAny<CancellationToken>())).ReturnsAsync(clientStream);
                var streamProducer = new StreamProducer(clientMock.Object, channel, logMock.Object);

                // Act
                await streamProducer.ProduceAsync(CancellationToken.None);

                var result = await channel.Reader.ReadAsync();
                var result2 = await channel.Reader.ReadAsync();

                // Assert
                Assert.Equal("Hello World", result);
                Assert.Equal("Goodbye World", result2);
            }
        }
    }
}
