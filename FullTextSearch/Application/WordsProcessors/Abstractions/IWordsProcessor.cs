namespace FullTextSearch.Application.WordsProcessors;

internal interface IWordsProcessor
{
    IReadOnlyList<string> GetAndWords(string[] query);
    IReadOnlyList<string> GetOrWords(string[] query);
    IReadOnlyList<string> GetNotWords(string[] query);
}
