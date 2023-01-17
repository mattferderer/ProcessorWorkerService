namespace ProcessorWorkerService.Services.Producers
{
    public interface ISocialMediaProducer
    {
        Task ProduceAsync(CancellationToken cancellationToken);
    }
}