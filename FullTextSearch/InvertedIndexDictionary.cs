using CodeStar2.Interfaces;

namespace CodeStar2;

public class InvertedIndexDictionary(
    string filepath,
    IInvertedIndexDictionaryBuilder invertedIndexDictionaryBuilder,
    IQuerySearcher querySearcher) : IInvertedIndexDictionary
{
    
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
    private readonly IQuerySearcher  _querySearcher = querySearcher;

    public IEnumerable<string> Search(string query) => _querySearcher.Search(query, _invertedIndex);
}
