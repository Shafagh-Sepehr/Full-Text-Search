namespace FullTextSearch.Application.WordsProcessors;

public interface INotWordsProcessor
{
    List<string> GetNotWords(string[] queryWords);
}
