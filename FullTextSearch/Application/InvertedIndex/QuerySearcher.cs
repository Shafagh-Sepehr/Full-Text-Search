using FullTextSearch.Application.InvertedIndex.Interfaces;
using FullTextSearch.Application.Searchers;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.WordsProcessors.Interfaces;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex;

internal class QuerySearcher(IWordsProcessor wordsProcessor, ISearcher searcher)
    : IQuerySearcher
{
    private ISearcher                         Searcher       { get; } = searcher;
    private IWordsProcessor                   WordsProcessor { get; } = wordsProcessor;
    private Dictionary<string, List<string>>? InvertedIndex  { get; set; }
    private string[]                          QueryWords     { get; set; } = null!;
    private bool                              IsConstructed  { get; set; }

    private List<string> AndWords => WordsProcessor.GetAndWords(QueryWords);
    private List<string> OrWords  => WordsProcessor.GetOrWords(QueryWords);
    private List<string> NotWords => WordsProcessor.GetNotWords(QueryWords);

    private Words Words => new()
    {
        AndWords = AndWords,
        OrWords = OrWords,
        NotWords = NotWords,
    };


    public void Construct(Dictionary<string, List<string>> invertedIndex)
    {
        InvertedIndex = invertedIndex;

        IsConstructed = true;
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
        if (!IsConstructed) throw new ConstructMethodNotCalledException();
    }

    private IEnumerable<string> ExecuteSearch(IEnumerable<string> result)
    {
        if (AreAllWordTypesPresent())
            result = Searcher.AndOrNotSearch(InvertedIndex!, Words);
        else if (AreAndWordsPresent())
            result = Searcher.AndNotSearch(InvertedIndex!, Words);
        else if (AreOrWordsPresent()) result = Searcher.OrNotSearch(InvertedIndex!, Words);

        return result;
    }

    private bool AreAllWordTypesPresent() => OrWords.Count > 0 && AndWords.Count > 0;
    private bool AreAndWordsPresent() => AndWords.Count > 0;
    private bool AreOrWordsPresent() => OrWords.Count > 0;

    private void SetQueryWords(string query) => QueryWords = query.Trim().Split();
}
