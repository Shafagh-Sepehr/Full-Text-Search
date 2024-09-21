using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

public class InvertedIndexDictionary
{
    private readonly Dictionary<string, List<string>> _invertedIndex;

    private readonly IQuerySearcher  _querySearcher;
    private readonly IPorter2Stemmer _stemmer = new EnglishPorter2Stemmer();

    public InvertedIndexDictionary(string filepath, IInvertedIndexDictionaryBuilder invertedIndexDictionaryBuilder,
                                   IQuerySearcher querySearcher)
    {
        _querySearcher = querySearcher;
        _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
    }

    public IEnumerable<string> Search(string query) => _querySearcher.Search(query, _invertedIndex);
}
