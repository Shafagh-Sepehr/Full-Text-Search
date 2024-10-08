namespace FullTextSearch.Application.QueryProcessor.Interfaces;

public interface IAndWordsProcessor
{
    List<string> GetAndWords(string[] queryWords);
}
