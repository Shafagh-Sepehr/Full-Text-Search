namespace FullTextSearch.Application.WordsProcessors.Interfaces;

public interface IOrWordsProcessor
{
    List<string> GetOrWords(string[] queryWords);
}
