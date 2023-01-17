using ProcessorWorkerService.Services.DataStores;

namespace ProcessorTests.Services.DataStores
{
    public class SimpleMemoryStoreTests
    {
        [Fact]
        public void SimpleMemoryStoreKeepsCountAndSortOfTopHashTags()
        {
            // Arrange
            var dataStore = new SimpleMemoryStore();

            // Act
            foreach (var hashTags in HashTagGenerator())
            {
                // This could be broken into two different tests, doing only one for now
                dataStore.IncrementPostCount();
                dataStore.AddHashTags(hashTags);
            }

            // Assert
            Assert.Equal(4, dataStore.GetPostCount());
            Assert.Equal("#Hello", dataStore.GetTopHashTags(2).First().Key);
            Assert.Equal(3, dataStore.GetTopHashTags(2).First().Value);
            Assert.Equal("#Test", dataStore.GetTopHashTags(2)[1].Key);
            Assert.Equal(2, dataStore.GetTopHashTags(2)[1].Value);
        }

        private IEnumerable<string[]> HashTagGenerator()
        {
            var hashTags = new List<string[]>()
            {
                new string[]
                {
                    "#Hello",
                    "#GoodBye",
                    "#Test",
                    "#Hashtag"
                },
                new string[]
                {
                    "#Abc",
                    "#Xyz",
                },
                new string[]
                {
                    "#Hello",
                },
                new string[]
                {
                    "#Hello",
                    "#Test"
                }
            };
            foreach (var hashTag in hashTags)
            {
                yield return hashTag;
            }
        }
    }
}
