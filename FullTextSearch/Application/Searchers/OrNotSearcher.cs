using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal sealed class OrNotSearcher(IDocumentReader documentReader) : IOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));

    public IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words)
    {
        var docsSet = _documentReader.GetOrDocuments(invertedIndex, words.OrWords);
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));

        return docsSet;
    }
}