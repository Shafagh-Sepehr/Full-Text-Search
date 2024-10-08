using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.InvertedIndex.Interfaces;
using FullTextSearch.Application.WordsProcessors;
using FullTextSearch.Application.WordsProcessors.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal class QuerySearcher : IQuerySearcher
{
    private readonly Dictionary<string, List<string>> _invertedIndex;
    private          string[]                         _queryWords    = null!;
    private readonly IWordsProcessor                  _wordsProcessor;

    private readonly IDocumentReader _documentReader;
    
    public QuerySearcher(Dictionary<string, List<string>> invertedIndex,
                         IPorter2Stemmer? injectedStemmer = null, IDocumentReader? documentReader = null)
    {
        IPorter2Stemmer stemmer = injectedStemmer ?? new EnglishPorter2Stemmer();
        _wordsProcessor = new WordsProcessor(stemmer);
        _invertedIndex = invertedIndex;
        
        _documentReader = documentReader ?? new DocumentReader(new AndDocumentsReader(invertedIndex),
                                                               new OrDocumentsReader(invertedIndex),
                                                               new NotDocumentsReader(invertedIndex));
    }

    public IEnumerable<string> Search(string query)
    {
        IEnumerable<string> result = [];
        
        if (string.IsNullOrWhiteSpace(query))
            return result;

        SetQueryWords(query);        
        
        if (AreAllWordTypesPresent())
        {
            result = AndOrNotSearch();
        }
        else if (AreAndWordsPresent())
        {
            result = AndNotSearch();
        }
        else if (AreOrWordsPresent())
        {
            result = OrNotSearch();
        }
        
        return result;
    }
    
    private IEnumerable<string> AndOrNotSearch() =>
        _documentReader.GetAndDocuments(AndWords)
            .Intersect(_documentReader.GetOrDocuments(OrWords))
            .Except(_documentReader.GetNotDocuments(NotWords));
    
    private IEnumerable<string> AndNotSearch() =>
        _documentReader.GetAndDocuments(AndWords)
            .Except(_documentReader.GetNotDocuments(NotWords));

    private IEnumerable<string> OrNotSearch() =>
        _documentReader.GetOrDocuments(OrWords)
            .Except(_documentReader.GetNotDocuments(NotWords));
    
    

    private bool AreAllWordTypesPresent() => OrWords.Count > 0 && AndWords.Count > 0;
    private bool AreAndWordsPresent() => AndWords.Count > 0;
    private bool AreOrWordsPresent() => OrWords.Count > 0;
    
    private void SetQueryWords(string query) => _queryWords = query.Trim().Split();
    
    private List<string> AndWords => _wordsProcessor.GetAndWords(_queryWords);
    private List<string> OrWords => _wordsProcessor.GetOrWords(_queryWords);
    private List<string> NotWords => _wordsProcessor.GetNotWords(_queryWords);
}
