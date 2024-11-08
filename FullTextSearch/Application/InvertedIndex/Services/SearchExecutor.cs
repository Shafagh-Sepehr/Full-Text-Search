using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal class SearchExecutor(ISearcher searcher) : ISearchExecutor
{
    private readonly ISearcher                                  _searcher = searcher ?? throw new ArgumentNullException(nameof(searcher));
    private          IReadOnlyDictionary<string, List<string>>? _invertedIndex;
    private          bool                                       _isConstructed;
    
    public void Construct(IReadOnlyDictionary<string, List<string>> invertedIndex)
    {
        _invertedIndex = invertedIndex;
        
        _isConstructed = true;
    }
    
    public IReadOnlySet<string> ExecuteSearch(ProcessedQueryWords processedWords)
    {
        AssertConstructMethodCalled();

        
        if (AreAllWordTypesPresent(processedWords))
            return _searcher.AndOrNotSearch(_invertedIndex!, processedWords);
        if (AreAndWordsPresent(processedWords))
            return _searcher.AndNotSearch(_invertedIndex!, processedWords);
        if (AreOrWordsPresent(processedWords))
            return _searcher.OrNotSearch(_invertedIndex!, processedWords);
        return new HashSet<string>();
    }
    
    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed) throw new ConstructMethodNotCalledException();
    }
    
    private bool AreAllWordTypesPresent(ProcessedQueryWords words) => words.OrWords.Count > 0 && words.AndWords.Count > 0;
    private bool AreAndWordsPresent(ProcessedQueryWords words) => words.AndWords.Count > 0;
    private bool AreOrWordsPresent(ProcessedQueryWords words) => words.OrWords.Count > 0;
}
