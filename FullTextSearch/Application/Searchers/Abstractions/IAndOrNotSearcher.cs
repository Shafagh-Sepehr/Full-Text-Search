using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IAndOrNotSearcher
{
    IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords);
}
