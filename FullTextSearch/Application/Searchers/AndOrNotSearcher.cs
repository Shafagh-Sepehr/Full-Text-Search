using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
{
    private IDocumentReader DocumentReader { get; } = documentReader;

    public IEnumerable<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        DocumentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Intersect(DocumentReader.GetOrDocuments(invertedIndex, words.OrWords))
            .Except(DocumentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
