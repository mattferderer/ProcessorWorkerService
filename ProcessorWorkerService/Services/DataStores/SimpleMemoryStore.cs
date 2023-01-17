using System.Collections.Concurrent;

namespace ProcessorWorkerService.Services.DataStores
{
    public class SimpleMemoryStore : ISocialMediaStore
    {
        private ConcurrentDictionary<string, int> _hashtagStore = new ConcurrentDictionary<string, int>();
        private int tweetCount = 0;

        public int GetPostCount() => tweetCount;

        public void IncrementPostCount() => Interlocked.Increment(ref tweetCount);


        public void AddHashTags(string[] hashTags)
        {
            foreach (var hashtag in hashTags)
            {
                _hashtagStore.AddOrUpdate(hashtag, 1, (key, oldVal) => oldVal + 1);
            }
        }

        public KeyValuePair<string, int>[] GetTopHashTags(int count) => _hashtagStore
            .ToArray() // clone snapshot as array before order
            .OrderByDescending(pair => pair.Value)
            .Take(count)
            .ToArray();
    }
}
