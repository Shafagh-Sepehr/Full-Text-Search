using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal sealed class Searcher(
    IAndOrNotSearcher andOrNotSearcher,
    IAndNotSearcher andNotSearcher,
    IOrNotSearcher orNotSearcher) : ISearcher
{
    private readonly IAndNotSearcher   _andNotSearcher   = andNotSearcher ?? throw new ArgumentNullException(nameof(andNotSearcher));
    private readonly IAndOrNotSearcher _andOrNotSearcher = andOrNotSearcher ?? throw new ArgumentNullException(nameof(andOrNotSearcher));
    private readonly IOrNotSearcher    _orNotSearcher    = orNotSearcher ?? throw new ArgumentNullException(nameof(orNotSearcher));

    public IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, words);

    public IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andNotSearcher.AndNotSearch(invertedIndex, words);

    public IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _orNotSearcher.OrNotSearch(invertedIndex, words);
}
