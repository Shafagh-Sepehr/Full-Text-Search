namespace FullTextSearch.Application.WordsProcessors;

internal interface INotWordsProcessor
{
    List<string> GetNotWords(string[] queryWords);
}
