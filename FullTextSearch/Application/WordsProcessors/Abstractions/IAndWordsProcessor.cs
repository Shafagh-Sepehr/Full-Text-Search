namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface IAndWordsProcessor
{
    IReadOnlyList<string> GetAndWords(string[] queryWords);
}
