namespace FullTextSearch.Application.QueryProcessor.Interfaces;

public interface IWordsProcessor
{
    List<string> GetAndWords(string[] query);
    List<string> GetOrWords(string[] query);
    List<string> GetNotWords(string[] query);
}
