using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

public class InvertedIndexDictionary(IQuerySearcher querySearcher, IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller)
    : IInvertedIndexDictionary
{
    private readonly IQuerySearcher                 _querySearcher                 = querySearcher;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller = invertedIndexDictionaryFiller;

    public void Construct(string path,IEnumerable<string>? banned)
    {
        _invertedIndexDictionaryFiller.Construct(banned);
        var invertedIndex = _invertedIndexDictionaryFiller.Build(path);
        _querySearcher.Construct(invertedIndex);
    }


    public IEnumerable<string> Search(string query) => _querySearcher.Search(query);
}
