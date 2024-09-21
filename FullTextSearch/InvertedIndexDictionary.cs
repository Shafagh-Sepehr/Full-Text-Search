using Porter2Stemmer;

namespace CodeStar2;

public class InvertedIndexDictionary
{
    private readonly IPorter2Stemmer                  _stemmer = new EnglishPorter2Stemmer();
    private readonly Dictionary<string, List<string>> _invertedIndex;
    
    public InvertedIndexDictionary(string filepath, IInvertedIndexDictionaryBuilder invertedIndexDictionaryBuilder
                                 , IEnumerable<string>? banned = null)
        => _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);

    public IEnumerable<string> Search(string query) => QuerySearcher.Search(query, _invertedIndex, _stemmer);
}
