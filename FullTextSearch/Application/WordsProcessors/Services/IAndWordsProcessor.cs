namespace FullTextSearch.Application.WordsProcessors;

internal interface IAndWordsProcessor
{
    IReadOnlyList<string> GetAndWords(string[] queryWords);
}
