namespace FullTextSearch.Application.WordsProcessors;

internal interface INotWordsProcessor
{
    IReadOnlyList<string> GetNotWords(string[] queryWords);
}
