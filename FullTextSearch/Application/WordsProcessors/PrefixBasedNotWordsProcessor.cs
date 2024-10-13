using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

internal class PrefixBasedNotWordsProcessor(IPorter2Stemmer stemmer) : INotWordsProcessor
{
    private readonly IPorter2Stemmer _stemmer = stemmer ?? throw new ArgumentNullException(nameof(stemmer));

    public List<string> GetNotWords(string[] queryWords)
    {
        return queryWords
            .Where(x => x[0] == '-')
            .Select(x => x.Substring(1, x.Length - 1))
            .Select(x => _stemmer.Stem(x).Value)
            .ToList();
    }
}
