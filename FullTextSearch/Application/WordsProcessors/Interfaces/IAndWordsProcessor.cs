namespace FullTextSearch.Application.WordsProcessors.Interfaces;

public interface IAndWordsProcessor
{
    List<string> GetAndWords(string[] queryWords);
}
