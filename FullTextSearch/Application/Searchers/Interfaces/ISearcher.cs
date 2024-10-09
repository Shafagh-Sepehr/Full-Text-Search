using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal interface ISearcher
{
    IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
    IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
    IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
