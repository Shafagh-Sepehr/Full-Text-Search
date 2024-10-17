namespace FullTextSearch.Application.WordsProcessors.Abstractions;

internal interface IOrWordsProcessor
{
    IReadOnlyList<string> GetOrWords(string[] queryWords);
}
