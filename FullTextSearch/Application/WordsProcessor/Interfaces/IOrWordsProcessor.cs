namespace FullTextSearch.Application.WordsProcessor.Interfaces;

public interface IOrWordsProcessor
{
    List<string> GetOrWords(string[] queryWords);
}
