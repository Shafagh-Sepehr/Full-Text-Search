using FullTextSearch.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch;

public class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly Dictionary<string, List<string>> _invertedIndex;
    private readonly IQuerySearcher                   _querySearcher;

    public InvertedIndexDictionary(string filepath, IEnumerable<string>? banned,
                                   IInvertedIndexDictionaryBuilder? invertedIndexDictionaryBuilder = null,
                                   IQuerySearcher? querySearcher = null)
    {
        var englishPorter2Stemmer = new EnglishPorter2Stemmer();
        var stringToWordsProcessor = new StringToWordsProcessor(banned, englishPorter2Stemmer);
        invertedIndexDictionaryBuilder ??= new InvertedIndexDictionaryBuilder(stringToWordsProcessor);
        
        
        _querySearcher = querySearcher ?? new QuerySearcher();
        
        _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
    }


    public IEnumerable<string> Search(string query) => _querySearcher.Search(query, _invertedIndex);
}
