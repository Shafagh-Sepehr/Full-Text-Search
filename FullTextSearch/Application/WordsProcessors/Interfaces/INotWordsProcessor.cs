namespace FullTextSearch.Application.WordsProcessors.Interfaces;

public interface INotWordsProcessor
{
    List<string> GetNotWords(string[] queryWords);
}
