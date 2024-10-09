namespace FullTextSearch.Application.WordsProcessors;

public interface IOrWordsProcessor
{
    List<string> GetOrWords(string[] queryWords);
}
