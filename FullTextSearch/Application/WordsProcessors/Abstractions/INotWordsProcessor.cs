namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface INotWordsProcessor
{
    IReadOnlyList<string> GetNotWords(IReadOnlyList<string> queryWords);
}
