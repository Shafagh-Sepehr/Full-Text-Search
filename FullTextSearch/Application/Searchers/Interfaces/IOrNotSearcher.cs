using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal interface IOrNotSearcher
{
    IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
