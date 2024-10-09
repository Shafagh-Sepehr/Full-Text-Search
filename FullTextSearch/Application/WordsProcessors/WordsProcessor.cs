using FullTextSearch.Application.WordsProcessors.Interfaces;

namespace FullTextSearch.Application.WordsProcessors;

public class WordsProcessor(
    IAndWordsProcessor andWordsProcessor,
    IOrWordsProcessor orWordsProcessor,
    INotWordsProcessor notWordsProcessor)
    : IWordsProcessor
{
    
    private readonly IAndWordsProcessor _andWordsProcessor = andWordsProcessor;
    private readonly IOrWordsProcessor  _orWordsProcessor  = orWordsProcessor;
    private readonly INotWordsProcessor _notWordsProcessor = notWordsProcessor;


    public List<string> GetAndWords(string[] query) => _andWordsProcessor.GetAndWords(query);
    public List<string> GetOrWords(string[] query) => _orWordsProcessor.GetOrWords(query);
    public List<string> GetNotWords(string[] query) => _notWordsProcessor.GetNotWords(query);

}
