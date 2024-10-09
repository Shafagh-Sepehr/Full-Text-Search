using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers.Interfaces;

internal interface IAndOrNotSearcher
{
    IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
