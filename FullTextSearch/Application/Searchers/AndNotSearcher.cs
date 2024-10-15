using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal sealed class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));


    public IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words)
    {
        var docsSet = _documentReader.GetAndDocuments(invertedIndex, words.AndWords);
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
        
        return docsSet;
    }
}
