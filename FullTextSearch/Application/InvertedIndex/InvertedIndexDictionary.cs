using FullTextSearch.Exceptions;

namespace FullTextSearch.Application.InvertedIndex;

public class InvertedIndexDictionary(
    IQuerySearcher querySearcher,
    IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller)
    : IInvertedIndexDictionary
{
    private IQuerySearcher                 Searcher              { get; } = querySearcher;
    private IInvertedIndexDictionaryFiller IndexDictionaryFiller { get; } = invertedIndexDictionaryFiller;
    private bool                           IsConstructed         { get; set; }

    public void Construct(string path, IEnumerable<string>? banned)
    {
        IndexDictionaryFiller.Construct(banned);
        var invertedIndex = IndexDictionaryFiller.Build(path);
        Searcher.Construct(invertedIndex);

        IsConstructed = true;
    }

    public IEnumerable<string> Search(string query)
    {
        AssertConstructMethodCalled();
        return Searcher.Search(query);
    }

    private void AssertConstructMethodCalled()
    {
        if (!IsConstructed) throw new ConstructMethodNotCalledException();
    }
}
