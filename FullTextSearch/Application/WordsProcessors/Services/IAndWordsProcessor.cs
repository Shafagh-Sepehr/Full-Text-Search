namespace FullTextSearch.Application.WordsProcessors;

internal interface IAndWordsProcessor
{
    List<string> GetAndWords(string[] queryWords);
}
