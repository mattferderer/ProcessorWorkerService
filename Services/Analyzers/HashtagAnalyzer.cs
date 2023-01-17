namespace ProcessorWorkerService.Services.Analyzers
{
    public static class HashtagAnalyzer
    {
        public static string[] GetHashTags(string tweet) => tweet.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Where(word => word.StartsWith("#"))
            .ToArray();
    }
}
