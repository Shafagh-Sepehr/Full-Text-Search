using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.Searchers.DataViewModels;
using FullTextSearch.Application.Searchers.Interfaces;

namespace FullTextSearch.Application.Searchers;

internal class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private IDocumentReader DocumentReader { get; } = documentReader;


    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        DocumentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Except(DocumentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
