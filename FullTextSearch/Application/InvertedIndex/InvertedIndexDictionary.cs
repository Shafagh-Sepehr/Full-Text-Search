using FullTextSearch.Application.InvertedIndex.Interfaces;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex;

public class InvertedIndexDictionary(IQuerySearcher querySearcher, IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller)
    : IInvertedIndexDictionary
{
    private readonly IQuerySearcher                 _querySearcher                 = querySearcher;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller = invertedIndexDictionaryFiller;
    private          bool                           _isConstructed;

    public void Construct(string path,IEnumerable<string>? banned)
    {
        _invertedIndexDictionaryFiller.Construct(banned);
        var invertedIndex = _invertedIndexDictionaryFiller.Build(path);
        _querySearcher.Construct(invertedIndex);

        _isConstructed = true;
    }

    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed)
        {
            throw new ConstructMethodNotCalledException();
        }
    }
    public IEnumerable<string> Search(string query)
    {
        AssertConstructMethodCalled();
        return _querySearcher.Search(query);
    }
}
