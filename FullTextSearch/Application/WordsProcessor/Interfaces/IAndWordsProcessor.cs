namespace FullTextSearch.Application.WordsProcessor.Interfaces;

public interface IAndWordsProcessor
{
    List<string> GetAndWords(string[] queryWords);
}
