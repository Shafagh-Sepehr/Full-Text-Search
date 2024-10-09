using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

public class PrefixBasedOrWordsProcessor(IPorter2Stemmer stemmer) : IOrWordsProcessor
{
    private readonly IPorter2Stemmer _stemmer = stemmer;

    public List<string> GetOrWords(string[] queryWords)
    {
        return queryWords
            .Where(x => x[0] == '+')
            .Select(x => x.Substring(1, x.Length - 1))
            .Select(x => _stemmer.Stem(x).Value)
            .ToList();
    }
}
