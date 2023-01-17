namespace ProcessorWorkerService.Services.Consumers
{
    public interface ISocialMediaConsumer
    {
        IAsyncEnumerable<string> ConsumeAsync(CancellationToken cancellationToken);
    }
}