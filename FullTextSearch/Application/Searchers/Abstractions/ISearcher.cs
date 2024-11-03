using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface ISearcher
{
    IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
    IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
    IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
}
