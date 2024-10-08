using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.DocumentsReader.Interfaces;
using FullTextSearch.Application.InvertedIndex.Interfaces;
using FullTextSearch.Application.WordsProcessor.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal class QuerySearcher : IQuerySearcher
{
    private readonly Dictionary<string, List<string>> _invertedIndex;
    private          string[]                         _queryWords    = null!;
    private readonly IWordsProcessor                  _wordsProcessor;

    private IDocumentReader _documentReader;
    
    public QuerySearcher(Dictionary<string, List<string>> invertedIndex, IPorter2Stemmer? injectedStemmer = null)
    {
        IPorter2Stemmer stemmer = injectedStemmer ?? new EnglishPorter2Stemmer();
        _wordsProcessor = new WordsProcessor.WordsProcessor(stemmer);
        _invertedIndex = invertedIndex;
        
        _documentReader = new DocumentReader(new AndDocumentsReader(invertedIndex,AndWords),
                                            new OrDocumentsReader(invertedIndex,AndWords),
                                            new NotDocumentsReader(invertedIndex,AndWords));
    }

    public IEnumerable<string> Search(string query)
    {
        IEnumerable<string> result = [];
        
        if (string.IsNullOrWhiteSpace(query))
            return result;

        _queryWords = query.Trim().Split();
       


        if (OrWords.Count > 0 && AndWords.Count > 0)
        {
            result = GetDocumentsForAndQueries().Intersect(GetDocumentsForOrQueries())
                .Except(GetDocumentsForNotQueries());
        }
        else if (AndWords.Count > 0)
        {
            result = GetDocumentsForAndQueries().Except(GetDocumentsForNotQueries());
        }
        else if (OrWords.Count > 0)
        {
            result = GetDocumentsForOrQueries().Except(GetDocumentsForNotQueries());
        }
        
        return result;
    }
    
    private List<string> AndWords => _wordsProcessor.GetAndWords(_queryWords);
    private List<string> OrWords => _wordsProcessor.GetOrWords(_queryWords);
    private List<string> NotWords => _wordsProcessor.GetNotWords(_queryWords);


    
}
