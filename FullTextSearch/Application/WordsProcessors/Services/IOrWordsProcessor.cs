namespace FullTextSearch.Application.WordsProcessors;

internal interface IOrWordsProcessor
{
    List<string> GetOrWords(string[] queryWords);
}
