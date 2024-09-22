using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

public class InvertedIndexDictionary(
    string filepath,
    IInvertedIndexDictionaryBuilder invertedIndexDictionaryBuilder,
    IQuerySearcher querySearcher) : IInvertedIndexDictionary
{
    
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
    private readonly IQuerySearcher  _querySearcher = querySearcher;
    private readonly IPorter2Stemmer _stemmer       = new EnglishPorter2Stemmer();

    public IEnumerable<string> Search(string query) => _querySearcher.Search(query, _invertedIndex);
}
