using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

public class PrefixBasedNotWordsProcessor(IPorter2Stemmer stemmer) : INotWordsProcessor
{
    private IPorter2Stemmer Stemmer { get; } = stemmer;

    public List<string> GetNotWords(string[] queryWords)
    {
        return queryWords
            .Where(x => x[0] == '-')
            .Select(x => x.Substring(1, x.Length - 1))
            .Select(x => Stemmer.Stem(x).Value)
            .ToList();
    }
}
