namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface INotWordsProcessor
{
    IReadOnlyList<string> GetNotWords(string[] queryWords);
}
