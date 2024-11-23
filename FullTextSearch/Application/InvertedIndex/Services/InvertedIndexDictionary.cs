using FullTextSearch.Application.InvertedIndex.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly IQuerySearcher                 _searcher;

    public InvertedIndexDictionary(IQuerySearcher querySearcher,
                                   IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller,
                                   IAppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        ArgumentNullException.ThrowIfNull(invertedIndexDictionaryFiller);
        _searcher = querySearcher ?? throw new ArgumentNullException(nameof(querySearcher));

        var invertedIndex = invertedIndexDictionaryFiller.Build(appSettings.documentsPath);
        _searcher.Construct(invertedIndex);
    }


    public IEnumerable<string> Search(string query) => _searcher.Search(query);
}
