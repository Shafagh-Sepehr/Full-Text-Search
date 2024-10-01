using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

public class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly Dictionary<string, List<string>> _invertedIndex;
    private readonly IQuerySearcher                   _querySearcher;

    public InvertedIndexDictionary(string filepath, IEnumerable<string>? banned,
                                   IInvertedIndexDictionaryBuilder? invertedIndexDictionaryBuilder = null,
                                   IQuerySearcher? querySearcher = null)
    {
        var stringToWordsProcessor = new StringToWordsProcessor(banned);
        var englishPorter2Stemmer = new EnglishPorter2Stemmer();
        invertedIndexDictionaryBuilder ??= new InvertedIndexDictionaryBuilder(stringToWordsProcessor, englishPorter2Stemmer);
        
        
        _querySearcher = querySearcher ?? new QuerySearcher();
        
        _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
    }


    public IEnumerable<string> Search(string query) => _querySearcher.Search(query, _invertedIndex);
}
