using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _documentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Intersect(_documentReader.GetOrDocuments(invertedIndex, words.OrWords))
            .Except(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
