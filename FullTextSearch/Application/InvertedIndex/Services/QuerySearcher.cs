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

    public void Construct(Dictionary<string, List<string>> invertedIndex)
    {
        _searchExecutor.Construct(invertedIndex);
        _isConstructed = true;
    }

    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed) throw new ConstructMethodNotCalledException();
    }

    public IEnumerable<string> Search(string query)
    {
        AssertConstructMethodCalled();
        if (string.IsNullOrWhiteSpace(query)) return [];

        SetQueryWords(query);
        return _searchExecutor.ExecuteSearch(ProcessedQueryWords);
    }

    private ProcessedQueryWords ProcessedQueryWords => new()
    {
        AndWords = _wordsProcessor.GetAndWords(_queryWords),
        OrWords = _wordsProcessor.GetOrWords(_queryWords),
        NotWords = _wordsProcessor.GetNotWords(_queryWords),
    };

    private void SetQueryWords(string query) => _queryWords = query.Trim().Split();
}
