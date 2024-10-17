using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface ISearcher
{
    IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords);
    IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords);
    IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords);
}
