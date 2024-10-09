using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
{
    private IDocumentReader DocumentReader { get; } = documentReader;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        DocumentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Intersect(DocumentReader.GetOrDocuments(invertedIndex, words.OrWords))
            .Except(DocumentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
