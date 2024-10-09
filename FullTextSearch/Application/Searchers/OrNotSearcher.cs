using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class OrNotSearcher(IDocumentReader documentReader) : IOrNotSearcher
{
    private IDocumentReader DocumentReader { get; } = documentReader;

    public IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        DocumentReader.GetOrDocuments(invertedIndex, words.OrWords)
            .Except(DocumentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
