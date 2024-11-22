using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.ConfigurationService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Application.InvertedIndex.Services;

public sealed class InvertedIndexFactory : IInvertedIndexFactory
{
    private readonly IReadOnlyList<string>? _bannedWords;
    private readonly string?       _documentsPath;
    public InvertedIndexFactory()
    {
        var serviceProvider = ServiceCollection.ServiceProvider;
        var config = serviceProvider.GetService<IConfigurationService>();
        ArgumentNullException.ThrowIfNull(config);

        _bannedWords = config.GetConfig().GetSection("BannedWords").Get<IReadOnlyList<string>>();
        _documentsPath = config.GetConfig()["DocumentsPath"];

        ArgumentNullException.ThrowIfNull(_documentsPath,"document path");
    }
    public IInvertedIndexDictionary Create()
    {
        var serviceProvider = ServiceCollection.ServiceProvider;
        var invertedIndex = serviceProvider.GetService<IInvertedIndexDictionary>();
        ArgumentNullException.ThrowIfNull(invertedIndex);

        invertedIndex.Construct(_documentsPath!, _bannedWords);
        return invertedIndex;
    }
}
