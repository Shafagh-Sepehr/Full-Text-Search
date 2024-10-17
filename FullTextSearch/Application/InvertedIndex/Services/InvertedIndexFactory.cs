using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Application.InvertedIndex;

public sealed class InvertedIndexFactory : IInvertedIndexFactory
{
    public IInvertedIndexDictionary Create(string path, IEnumerable<string>? banned)
    {
        var serviceProvider = Services.ConfigureServices();
        
        var invertedIndex = serviceProvider.GetService<IInvertedIndexDictionary>();
        ArgumentNullException.ThrowIfNull(invertedIndex);
        invertedIndex.Construct(path, banned);

        return invertedIndex;
    }
}
