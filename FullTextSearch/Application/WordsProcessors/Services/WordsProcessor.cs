using FullTextSearch.Application.WordsProcessors.Abstractions;

namespace FullTextSearch.Application.WordsProcessors.Services;

internal sealed class WordsProcessor(
    IAndWordsProcessor andWordsProcessor,
    IOrWordsProcessor orWordsProcessor,
    INotWordsProcessor notWordsProcessor)
    : IWordsProcessor
{
    private readonly IAndWordsProcessor _andWordsProcessor = andWordsProcessor ?? throw new ArgumentNullException(nameof(andWordsProcessor));
    private readonly INotWordsProcessor _notWordsProcessor = notWordsProcessor ?? throw new ArgumentNullException(nameof(notWordsProcessor));
    private readonly IOrWordsProcessor  _orWordsProcessor  = orWordsProcessor ?? throw new ArgumentNullException(nameof(orWordsProcessor));


    public IReadOnlyList<string> GetAndWords(string[] query) => _andWordsProcessor.GetAndWords(query);
    public IReadOnlyList<string> GetOrWords(string[] query) => _orWordsProcessor.GetOrWords(query);
    public IReadOnlyList<string> GetNotWords(string[] query) => _notWordsProcessor.GetNotWords(query);
}
