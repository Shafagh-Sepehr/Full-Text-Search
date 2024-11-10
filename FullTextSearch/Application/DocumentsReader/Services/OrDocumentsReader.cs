using FullTextSearch.Application.DocumentsReader.Abstractions;

namespace FullTextSearch.Application.DocumentsReader.Services;

internal sealed class OrDocumentsReader : IOrDocumentsReader
{
    public HashSet<string> GetOrDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords)
    {
        return invertedIndex
            .Where(x => orWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
