using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.WordsProcessors;

public class WordsProcessor(IPorter2Stemmer stemmer,
                            IAndWordsProcessor? andWordsProcessor = null,
                            IOrWordsProcessor? orWordsProcessor = null,
                            INotWordsProcessor? notWordsProcessor = null) : IWordsProcessor
{
    
    private readonly IAndWordsProcessor _andWordsProcessor = andWordsProcessor ?? new PrefixBasedAndWordsProcessor(stemmer);
    private readonly IOrWordsProcessor _orWordsProcessor  = orWordsProcessor ?? new PrefixBasedOrWordsProcessor(stemmer);
    private readonly INotWordsProcessor _notWordsProcessor = notWordsProcessor ?? new PrefixBasedNotWordsProcessor(stemmer);

    
    public List<string> GetAndWords(string[] query) => _andWordsProcessor.GetAndWords(query);
    public List<string> GetOrWords(string[] query) => _orWordsProcessor.GetOrWords(query);
    public List<string> GetNotWords(string[] query) => _notWordsProcessor.GetNotWords(query);

}
