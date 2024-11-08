namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface IWordsProcessor
{
    IReadOnlyList<string> GetAndWords(IReadOnlyList<string> query);
    IReadOnlyList<string> GetOrWords(IReadOnlyList<string> query);
    IReadOnlyList<string> GetNotWords(IReadOnlyList<string> query);
}
