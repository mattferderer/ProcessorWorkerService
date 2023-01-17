namespace ProcessorWorkerService.Services.HttpClients
{
    public interface ISocialMediaClient
    {
        Task<Stream?> GetNewContentStream(CancellationToken cancellationToken);
    }
}