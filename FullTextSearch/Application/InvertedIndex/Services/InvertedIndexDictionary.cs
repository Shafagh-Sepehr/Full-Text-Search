using FullTextSearch.Application.InvertedIndex.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly IInvertedIndexDictionaryFiller _indexDictionaryFiller;
    private readonly IQuerySearcher                 _searcher;
    private readonly IAppSettings                   _appSettings;

    public InvertedIndexDictionary(IQuerySearcher querySearcher,
                                   IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller,
                                   IAppSettings appSettings)
    {
        _indexDictionaryFiller = invertedIndexDictionaryFiller ?? throw new ArgumentNullException(nameof(invertedIndexDictionaryFiller));
        _searcher = querySearcher ?? throw new ArgumentNullException(nameof(querySearcher));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

        ConstructLowerLevelServices();
    }


    private void ConstructLowerLevelServices()
    {
        _indexDictionaryFiller.Construct(_appSettings.bannedWords);
        var invertedIndex = _indexDictionaryFiller.Build(_appSettings.documentsPath);
        _searcher.Construct(invertedIndex);
    }

    public IEnumerable<string> Search(string query) => _searcher.Search(query);
}
