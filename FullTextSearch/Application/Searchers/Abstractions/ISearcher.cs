using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface ISearcher
{
    IReadOnlySet<string> AndOrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
    IReadOnlySet<string> AndNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
    IReadOnlySet<string> OrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
}
