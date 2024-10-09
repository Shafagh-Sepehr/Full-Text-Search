using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.InvertedIndex.Interfaces;
using FullTextSearch.Application.WordsProcessors;
using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal class QuerySearcher(IWordsProcessor wordsProcessor, IDocumentReader documentReader)
    : IQuerySearcher
{
    private readonly IDocumentReader                   _documentReader = documentReader;
    private          Dictionary<string, List<string>>? _invertedIndex;
    private readonly IWordsProcessor                   _wordsProcessor = wordsProcessor;
    private          string[]                          _queryWords     = null!;

    public void Construct(Dictionary<string, List<string>> invertedIndex)
    {
        _invertedIndex = invertedIndex;
    }

    private List<string> AndWords => _wordsProcessor.GetAndWords(_queryWords);
    private List<string> OrWords  => _wordsProcessor.GetOrWords(_queryWords);
    private List<string> NotWords => _wordsProcessor.GetNotWords(_queryWords);

    public IEnumerable<string> Search(string query)
    {
        IEnumerable<string> result = [];

        if (string.IsNullOrWhiteSpace(query))
            return result;

        SetQueryWords(query);

        if (AreAllWordTypesPresent())
            result = AndOrNotSearch();
        else if (AreAndWordsPresent())
            result = AndNotSearch();
        else if (AreOrWordsPresent()) result = OrNotSearch();

        return result;
    }

    private IEnumerable<string> AndOrNotSearch() =>
        _documentReader.GetAndDocuments(_invertedIndex, AndWords)
            .Intersect(_documentReader.GetOrDocuments(_invertedIndex, OrWords))
            .Except(_documentReader.GetNotDocuments(_invertedIndex, NotWords));

    private IEnumerable<string> AndNotSearch() =>
        _documentReader.GetAndDocuments(_invertedIndex, AndWords)
            .Except(_documentReader.GetNotDocuments(_invertedIndex, NotWords));

    private IEnumerable<string> OrNotSearch() =>
        _documentReader.GetOrDocuments(_invertedIndex, OrWords)
            .Except(_documentReader.GetNotDocuments(_invertedIndex, NotWords));


    private bool AreAllWordTypesPresent() => OrWords.Count > 0 && AndWords.Count > 0;
    private bool AreAndWordsPresent() => AndWords.Count > 0;
    private bool AreOrWordsPresent() => OrWords.Count > 0;

    private void SetQueryWords(string query) => _queryWords = query.Trim().Split();
}
