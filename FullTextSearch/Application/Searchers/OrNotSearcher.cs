using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class OrNotSearcher(IDocumentReader documentReader) : IOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader;

    public IEnumerable<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _documentReader.GetOrDocuments(invertedIndex, words.OrWords)
            .Except(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
