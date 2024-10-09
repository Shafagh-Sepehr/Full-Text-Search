using FullTextSearch.Application.WordsProcessors.Interfaces;

namespace FullTextSearch.Application.WordsProcessors;

public class WordsProcessor(
    IAndWordsProcessor andWordsProcessor,
    IOrWordsProcessor orWordsProcessor,
    INotWordsProcessor notWordsProcessor)
    : IWordsProcessor
{
    private IAndWordsProcessor AndWordsProcessor { get; } = andWordsProcessor;
    private IOrWordsProcessor  OrWordsProcessor  { get; } = orWordsProcessor;
    private INotWordsProcessor NotWordsProcessor { get; } = notWordsProcessor;


    public List<string> GetAndWords(string[] query) => AndWordsProcessor.GetAndWords(query);
    public List<string> GetOrWords(string[] query) => OrWordsProcessor.GetOrWords(query);
    public List<string> GetNotWords(string[] query) => NotWordsProcessor.GetNotWords(query);
}
