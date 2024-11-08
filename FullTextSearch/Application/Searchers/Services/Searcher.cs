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
    
    public IReadOnlySet<string> AndOrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords) =>
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, processedQueryWords);
    
    public IReadOnlySet<string> AndNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords) =>
        _andNotSearcher.AndNotSearch(invertedIndex, processedQueryWords);
    
    public IReadOnlySet<string> OrNotSearch(IReadOnlyDictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords) =>
        _orNotSearcher.OrNotSearch(invertedIndex, processedQueryWords);
}
