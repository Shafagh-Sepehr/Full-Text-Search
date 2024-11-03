using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IAndNotSearcher
{
    IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords);
}
