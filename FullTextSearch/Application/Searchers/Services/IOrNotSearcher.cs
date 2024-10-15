using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal interface IOrNotSearcher
{
    IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}