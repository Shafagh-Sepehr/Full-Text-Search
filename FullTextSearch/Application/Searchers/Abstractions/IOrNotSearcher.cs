using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IOrNotSearcher
{
    IReadOnlySet<string> OrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
}
