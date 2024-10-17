using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;

namespace FullTextSearch.Application.Searchers.Services;

internal sealed class OrNotSearcher(IDocumentReader documentReader) : IOrNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));

    public IReadOnlySet<string> OrNotSearch(Dictionary<string, List<string>> invertedIndex, QueryProcessedWords queryProcessedWords)
    {
        var docsSet = _documentReader.GetOrDocuments(invertedIndex, queryProcessedWords.OrWords);
        docsSet.ExceptWith(_documentReader.GetNotDocuments(invertedIndex, queryProcessedWords.NotWords));

        return docsSet;
    }
}
