namespace FullTextSearch.Application.WordsProcessors;

internal interface IOrWordsProcessor
{
    IReadOnlyList<string> GetOrWords(string[] queryWords);
}
