using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _documentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Intersect(_documentReader.GetOrDocuments(invertedIndex, words.OrWords))
            .Except(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
