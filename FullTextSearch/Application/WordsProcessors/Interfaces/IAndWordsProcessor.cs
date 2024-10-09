namespace FullTextSearch.Application.WordsProcessors;

public interface IAndWordsProcessor
{
    List<string> GetAndWords(string[] queryWords);
}
