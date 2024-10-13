using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal interface IAndOrNotSearcher
{
    IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
