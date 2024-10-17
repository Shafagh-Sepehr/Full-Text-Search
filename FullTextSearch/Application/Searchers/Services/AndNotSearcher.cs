using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;

namespace FullTextSearch.Application.Searchers.Services;

internal sealed class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));


    public IReadOnlySet<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords)
    {
        var docsSet = _documentReader.GetAndDocuments(invertedIndex, queryProcessedWords.AndWords);
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, queryProcessedWords.NotWords));
        
        return docsSet;
    }
}
