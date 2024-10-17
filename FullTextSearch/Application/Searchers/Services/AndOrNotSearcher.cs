using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers.Services;

internal sealed class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));

    public IReadOnlySet<string> AndOrNotSearch(Dictionary<string, List<string>> invertedIndex, Words words)
    {
        var docsSet =  _documentReader.GetAndDocuments(invertedIndex, words.AndWords);
        docsSet.IntersectWith(_documentReader.GetOrDocuments(invertedIndex, words.OrWords));
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
        
        return docsSet;
    }
}
