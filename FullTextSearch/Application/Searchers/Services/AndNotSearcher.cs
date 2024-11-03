using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;

namespace FullTextSearch.Application.Searchers.Services;

internal sealed class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));


    public IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, ProcessedQueryWords processedQueryWords)
    {
        var docsSet = _documentReader.GetAndDocuments(invertedIndex, processedQueryWords.AndWords);
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, processedQueryWords.NotWords));
        
        return docsSet;
    }
}
