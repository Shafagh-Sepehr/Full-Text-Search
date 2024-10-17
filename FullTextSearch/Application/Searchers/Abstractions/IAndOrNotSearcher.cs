using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers.Abstractions;

internal interface IAndOrNotSearcher
{
    IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
