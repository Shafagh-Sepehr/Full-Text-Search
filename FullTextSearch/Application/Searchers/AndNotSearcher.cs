using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private readonly IDocumentReader _documentReader = documentReader;
    
    
    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        _documentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Except(_documentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
