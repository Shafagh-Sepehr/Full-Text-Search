namespace FullTextSearch.Application.WordsProcessors;

internal class WordsProcessor(
    IAndWordsProcessor andWordsProcessor,
    IOrWordsProcessor orWordsProcessor,
    INotWordsProcessor notWordsProcessor)
    : IWordsProcessor
{
    private readonly IAndWordsProcessor _andWordsProcessor = andWordsProcessor;
    private readonly INotWordsProcessor _notWordsProcessor = notWordsProcessor;
    private readonly IOrWordsProcessor  _orWordsProcessor  = orWordsProcessor;


    public List<string> GetAndWords(string[] query) => _andWordsProcessor.GetAndWords(query);
    public List<string> GetOrWords(string[] query) => _orWordsProcessor.GetOrWords(query);
    public List<string> GetNotWords(string[] query) => _notWordsProcessor.GetNotWords(query);
}
