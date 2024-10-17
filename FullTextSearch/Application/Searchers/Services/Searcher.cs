using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;

namespace FullTextSearch.Application.Searchers.Services;

internal sealed class Searcher(
    IAndOrNotSearcher andOrNotSearcher,
    IAndNotSearcher andNotSearcher,
    IOrNotSearcher orNotSearcher) : ISearcher
{
    private readonly IAndNotSearcher   _andNotSearcher   = andNotSearcher ?? throw new ArgumentNullException(nameof(andNotSearcher));
    private readonly IAndOrNotSearcher _andOrNotSearcher = andOrNotSearcher ?? throw new ArgumentNullException(nameof(andOrNotSearcher));
    private readonly IOrNotSearcher    _orNotSearcher    = orNotSearcher ?? throw new ArgumentNullException(nameof(orNotSearcher));

    public IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords) =>
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, queryProcessedWords);

    public IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords) =>
        _andNotSearcher.AndNotSearch(invertedIndex, queryProcessedWords);

    public IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords) =>
        _orNotSearcher.OrNotSearch(invertedIndex, queryProcessedWords);
}
