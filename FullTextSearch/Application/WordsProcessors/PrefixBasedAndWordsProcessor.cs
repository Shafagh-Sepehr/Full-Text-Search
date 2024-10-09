using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

public class PrefixBasedAndWordsProcessor(IPorter2Stemmer stemmer) : IAndWordsProcessor
{
    private readonly IPorter2Stemmer _stemmer = stemmer;

    public List<string> GetAndWords(string[] queryWords)
    {
        return queryWords
            .Where(x => x[0] != '+' && x[0] != '-')
            .Select(x => _stemmer.Stem(x).Value)
            .ToList();
    }
}
