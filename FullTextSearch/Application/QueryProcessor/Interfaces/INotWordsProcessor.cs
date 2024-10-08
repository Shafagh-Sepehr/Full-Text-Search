namespace FullTextSearch.Application.QueryProcessor.Interfaces;

public interface INotWordsProcessor
{
    List<string> GetNotWords(string[] queryWords);
}
