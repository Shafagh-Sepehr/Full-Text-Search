namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface IAndWordsProcessor
{
    IReadOnlyList<string> GetAndWords(IReadOnlyList<string> queryWords);
}
