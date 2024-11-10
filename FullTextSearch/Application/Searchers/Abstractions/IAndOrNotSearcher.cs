using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IAndOrNotSearcher
{
    IReadOnlySet<string> AndOrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
}
