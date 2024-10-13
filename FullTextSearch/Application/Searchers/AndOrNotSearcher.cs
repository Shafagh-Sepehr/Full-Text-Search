using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class AndOrNotSearcher(IDocumentReader documentReader) : IAndOrNotSearcher
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
