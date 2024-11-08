using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class InvertedIndexDictionary(
    IQuerySearcher querySearcher,
    IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller)
    : IInvertedIndexDictionary
{
    private readonly IInvertedIndexDictionaryFiller _indexDictionaryFiller =
        invertedIndexDictionaryFiller ?? throw new ArgumentNullException(nameof(invertedIndexDictionaryFiller));

    private readonly IQuerySearcher _searcher = querySearcher ?? throw new ArgumentNullException(nameof(querySearcher));
    private          bool           _isConstructed;

    public void Construct(string path, IReadOnlyList<string>? bannedWords = null)
    {
        _indexDictionaryFiller.Construct(bannedWords);
        var invertedIndex = _indexDictionaryFiller.Build(path);
        _searcher.Construct(invertedIndex);

        _isConstructed = true;
    }

    public IEnumerable<string> Search(string query)
    {
        AssertConstructMethodCalled();
        return _searcher.Search(query);
    }

    private void AssertConstructMethodCalled()
    {
        if (!_isConstructed) throw new ConstructMethodNotCalledException();
    }
}
