using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class Searcher(IAndOrNotSearcher andOrNotSearcher, IAndNotSearcher andNotSearcher, IOrNotSearcher orNotSearcher) : ISearcher
{
    private readonly IAndOrNotSearcher _andOrNotSearcher = andOrNotSearcher;
    private readonly IAndNotSearcher   _andNotSearcher   = andNotSearcher;
    private readonly IOrNotSearcher    _orNotSearcher    = orNotSearcher;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, words);
    
    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _andNotSearcher.AndNotSearch(invertedIndex, words);
    
    public IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _orNotSearcher.OrNotSearch(invertedIndex, words);
    
    
}
