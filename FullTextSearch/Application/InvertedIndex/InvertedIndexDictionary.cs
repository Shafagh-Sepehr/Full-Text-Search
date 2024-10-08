using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

public class InvertedIndexDictionary(IQuerySearcher querySearcher) : IInvertedIndexDictionary
{
    private readonly IQuerySearcher _querySearcher = querySearcher;


    public IEnumerable<string> Search(string query) => _querySearcher.Search(query);
}
