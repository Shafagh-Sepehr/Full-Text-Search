using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.Searchers.DataViewModels;

namespace FullTextSearch.Application.Searchers;

internal class AndNotSearcher(IDocumentReader documentReader) : IAndNotSearcher
{
    private IDocumentReader DocumentReader { get; } = documentReader;


    public IEnumerable<string> AndNotSearch(Dictionary<string, List<string>> invertedIndex, Words words) =>
        DocumentReader.GetAndDocuments(invertedIndex, words.AndWords)
            .Except(DocumentReader.GetNotDocuments(invertedIndex, words.NotWords));
}
