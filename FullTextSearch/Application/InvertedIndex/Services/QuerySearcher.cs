using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class QuerySearcher(IWordsProcessor wordsProcessor, ISearcher searcher)
    : IQuerySearcher
{
    private readonly ISearcher                         _searcher       = searcher ?? throw new ArgumentNullException(nameof(searcher));
    private readonly IWordsProcessor                   _wordsProcessor = wordsProcessor ?? throw new ArgumentNullException(nameof(wordsProcessor));
    private          Dictionary<string, List<string>>? _invertedIndex;
    private          bool                              _isConstructed;
    private          string[]                          _queryWords = null!;

    private IReadOnlyList<string> AndWords => _wordsProcessor.GetAndWords(_queryWords);
    private IReadOnlyList<string> OrWords  => _wordsProcessor.GetOrWords(_queryWords);
    private IReadOnlyList<string> NotWords => _wordsProcessor.GetNotWords(_queryWords);

    private QueryProcessedWords QueryProcessedWords => new()
    {
        AndWords = AndWords,
        OrWords = OrWords,
        NotWords = NotWords,
    };


    public void Construct(Dictionary<string, List<string>> invertedIndex)
    {
        _invertedIndex = invertedIndex;

        _isConstructed = true;
    }

    public IEnumerable<string> Search(string query)
    {
        AssertConstructMethodCalled();

        IEnumerable<string> result = [];

        if (string.IsNullOrWhiteSpace(query))
            return result;

        SetQueryWords(query);

        result = ExecuteSearch(result);

        return result;
    }

    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed) throw new ConstructMethodNotCalledException();
    }

    private IEnumerable<string> ExecuteSearch(IEnumerable<string> result)
    {
        if (AreAllWordTypesPresent())
            result = _searcher.AndOrNotSearch(_invertedIndex!, QueryProcessedWords);
        else if (AreAndWordsPresent())
            result = _searcher.AndNotSearch(_invertedIndex!, QueryProcessedWords);
        else if (AreOrWordsPresent()) result = _searcher.OrNotSearch(_invertedIndex!, QueryProcessedWords);

        return result;
    }

    private bool AreAllWordTypesPresent() => OrWords.Count > 0 && AndWords.Count > 0;
    private bool AreAndWordsPresent() => AndWords.Count > 0;
    private bool AreOrWordsPresent() => OrWords.Count > 0;

    private void SetQueryWords(string query) => _queryWords = query.Trim().Split();
}
