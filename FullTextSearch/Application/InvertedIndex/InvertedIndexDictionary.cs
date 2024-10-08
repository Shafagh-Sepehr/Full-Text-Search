using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

public class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly IQuerySearcher                   _querySearcher;

    public InvertedIndexDictionary(string filepath, IEnumerable<string>? banned,
                                   IInvertedIndexDictionaryBuilder? invertedIndexDictionaryBuilder = null,
                                   IQuerySearcher? querySearcher = null)
    {
        var englishPorter2Stemmer = new EnglishPorter2Stemmer();
        var stringToWordsProcessor = new StringToWordsProcessor(banned, englishPorter2Stemmer);
        invertedIndexDictionaryBuilder ??= new InvertedIndexDictionaryBuilder(stringToWordsProcessor);
        
        
        Dictionary<string, List<string>> invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
        
        _querySearcher = querySearcher ?? new QuerySearcher(invertedIndex);
    }


    public IEnumerable<string> Search(string query) => _querySearcher.Search(query);
}
