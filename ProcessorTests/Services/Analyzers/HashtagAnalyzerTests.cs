using ProcessorWorkerService.Services.Analyzers;

namespace ProcessorTests.Services.Analyzers
{
    public class HashtagAnalyzerTests
    {
        [Fact]
        public void HashtagAnalyzerCanGetHashTags()
        {
            var tweet = "This is a #test tweet with multiple #hashtags - as of now we're okay with duplicate #hashtags in one tweet. May want to change that.";
            var expected = new[] { "#test", "#hashtags", "#hashtags" };
            var actual = HashtagAnalyzer.GetHashTags(tweet);
            Assert.Equal(expected, actual);
        }

    }
}
