using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class Searcher(
    IAndOrNotSearcher andOrNotSearcher,
    IAndNotSearcher andNotSearcher,
    IOrNotSearcher orNotSearcher) : ISearcher
{
    private readonly IAndNotSearcher   _andNotSearcher   = andNotSearcher ?? throw new ArgumentNullException(nameof(andNotSearcher));
    private readonly IAndOrNotSearcher _andOrNotSearcher = andOrNotSearcher ?? throw new ArgumentNullException(nameof(andOrNotSearcher));
    private readonly IOrNotSearcher    _orNotSearcher    = orNotSearcher ?? throw new ArgumentNullException(nameof(orNotSearcher));

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, words);

    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andNotSearcher.AndNotSearch(invertedIndex, words);

    public IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _orNotSearcher.OrNotSearch(invertedIndex, words);
}
