using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IAndNotSearcher
{
    IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
