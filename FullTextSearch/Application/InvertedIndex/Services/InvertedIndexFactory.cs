using FullTextSearch.Application.InvertedIndex.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Application.InvertedIndex.Services;

public sealed class InvertedIndexFactory : IInvertedIndexFactory
{
    public IInvertedIndexDictionary Create()
    {
        var serviceProvider = ServiceCollection.ServiceProvider;
        var invertedIndex = serviceProvider.GetService<IInvertedIndexDictionary>();
        ArgumentNullException.ThrowIfNull(invertedIndex);

        return invertedIndex;
    }
}
