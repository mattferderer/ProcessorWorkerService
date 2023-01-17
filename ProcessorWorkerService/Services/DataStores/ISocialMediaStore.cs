namespace ProcessorWorkerService.Services.DataStores
{
    public interface ISocialMediaStore
    {
        void AddHashTags(string[] hashTags);
        KeyValuePair<string, int>[] GetTopHashTags(int count);
        int GetPostCount();
        void IncrementPostCount();
    }
}