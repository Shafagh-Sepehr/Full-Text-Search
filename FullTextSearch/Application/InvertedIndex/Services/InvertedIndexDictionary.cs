using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.ConfigurationService.Abstractions;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class InvertedIndexDictionary : IInvertedIndexDictionary
{
    private readonly IInvertedIndexDictionaryFiller _indexDictionaryFiller;
    private readonly IConfigurationService          _configurationService;
    private readonly IQuerySearcher                 _searcher;

    public InvertedIndexDictionary(IQuerySearcher querySearcher,
                                   IInvertedIndexDictionaryFiller invertedIndexDictionaryFiller,
                                   IConfigurationService configurationService)
    {
        _indexDictionaryFiller = invertedIndexDictionaryFiller ?? throw new ArgumentNullException(nameof(invertedIndexDictionaryFiller));
        _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        _searcher = querySearcher ?? throw new ArgumentNullException(nameof(querySearcher));

        ConstructLowerLevelServices();
    }


    private void ConstructLowerLevelServices()
    {
        _indexDictionaryFiller.Construct(GetBannedWords());
        var invertedIndex = _indexDictionaryFiller.Build(GetDocumentsPath());
        _searcher.Construct(invertedIndex);
    }

    public IEnumerable<string> Search(string query) => _searcher.Search(query);

    private IReadOnlyList<string>? GetBannedWords() => _configurationService.GetConfig().GetSection("BannedWords").Get<IReadOnlyList<string>>();
    private string GetDocumentsPath()
    {
        var documentsPath = _configurationService.GetConfig()["DocumentsPath"];
        ArgumentNullException.ThrowIfNull(documentsPath,"document path");
        return documentsPath;
    }
}
