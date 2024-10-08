namespace FullTextSearch.Application.WordsProcessor.Interfaces;

public interface INotWordsProcessor
{
    List<string> GetNotWords(string[] queryWords);
}
