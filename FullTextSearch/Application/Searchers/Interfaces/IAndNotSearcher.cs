using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers.Interfaces;

internal interface IAndNotSearcher
{
    IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
