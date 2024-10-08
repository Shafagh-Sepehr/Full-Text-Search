namespace FullTextSearch.Application.QueryProcessor.Interfaces;

public interface IOrWordsProcessor
{
    List<string> GetOrWords(string[] queryWords);
}
