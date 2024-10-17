using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IOrNotSearcher
{
    IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords);
}
