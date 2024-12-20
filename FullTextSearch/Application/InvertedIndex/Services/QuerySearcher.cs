using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class QuerySearcher(IWordsProcessor wordsProcessor, ISearchExecutor searchExecutor) : IQuerySearcher
{
    private readonly ISearchExecutor _searchExecutor = searchExecutor ?? throw new ArgumentNullException(nameof(searchExecutor));
    private readonly IWordsProcessor _wordsProcessor = wordsProcessor ?? throw new ArgumentNullException(nameof(wordsProcessor));
    private          bool            _isConstructed;
    private          string[]        _queryWords = null!;
    
    private ProcessedQueryWords ProcessedQueryWords => new()
    {
        AndWords = _wordsProcessor.GetAndWords(_queryWords),
        OrWords = _wordsProcessor.GetOrWords(_queryWords),
        NotWords = _wordsProcessor.GetNotWords(_queryWords),
    };
    
    public void Construct(IReadOnlyDictionary<string, List<string>> invertedIndex)
    {
        _searchExecutor.Construct(invertedIndex);
        _isConstructed = true;
    }
    
    public IReadOnlySet<string> Search(string query)
    {
        AssertConstructMethodCalled();
        if (string.IsNullOrWhiteSpace(query)) return new HashSet<string>();
        
        SetQueryWords(query);
        return _searchExecutor.ExecuteSearch(ProcessedQueryWords);
    }
    
    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed) throw new ConstructMethodNotCalledException();
    }
    
    private void SetQueryWords(string query) => _queryWords = query.Trim().Split();
}
