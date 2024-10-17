using FullTextSearch.Application.InvertedIndex.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Application.InvertedIndex.Services;

public sealed class InvertedIndexFactory : IInvertedIndexFactory
{
    public IInvertedIndexDictionary Create(string path, IEnumerable<string>? banned)
    {
        var serviceProvider = FullTextSearch.ServiceCollection.ServiceProvider;
        
        var invertedIndex = serviceProvider.GetService<IInvertedIndexDictionary>();
        ArgumentNullException.ThrowIfNull(invertedIndex);
        invertedIndex.Construct(path, banned);

        return invertedIndex;
    }
}
