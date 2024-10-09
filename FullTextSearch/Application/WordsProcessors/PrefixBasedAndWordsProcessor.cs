using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

public class PrefixBasedAndWordsProcessor(IPorter2Stemmer stemmer) : IAndWordsProcessor
{
    private IPorter2Stemmer Stemmer { get; } = stemmer;

    public List<string> GetAndWords(string[] queryWords)
    {
        return queryWords
            .Where(x => x[0] != '+' && x[0] != '-')
            .Select(x => Stemmer.Stem(x).Value)
            .ToList();
    }
}
