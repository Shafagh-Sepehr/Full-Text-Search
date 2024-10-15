using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal interface ISearcher
{
    IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
    IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
    IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words);
}
