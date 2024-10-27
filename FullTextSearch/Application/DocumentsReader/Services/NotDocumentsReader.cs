using FullTextSearch.Application.DocumentsReader.Abstractions;

namespace FullTextSearch.Application.DocumentsReader.Services;

internal sealed class NotDocumentsReader : INotDocumentsReader
{
    public HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords)
    {
        return invertedIndex
            .Where(x => notWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}