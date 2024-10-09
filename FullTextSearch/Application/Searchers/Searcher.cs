using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class Searcher(
    IAndOrNotSearcher andOrNotSearcher,
    IAndNotSearcher andNotSearcher,
    IOrNotSearcher orNotSearcher) : ISearcher
{
    private IAndOrNotSearcher AndOrNotSearcher { get; } = andOrNotSearcher;
    private IAndNotSearcher   AndNotSearcher   { get; } = andNotSearcher;
    private IOrNotSearcher    OrNotSearcher    { get; } = orNotSearcher;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        AndOrNotSearcher.AndOrNotSearch(invertedIndex, words);

    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        AndNotSearcher.AndNotSearch(invertedIndex, words);

    public IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        OrNotSearcher.OrNotSearch(invertedIndex, words);
}
