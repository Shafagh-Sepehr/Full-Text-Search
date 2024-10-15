namespace FullTextSearch.Application.DocumentsReader;

internal sealed class OrDocumentsReader : IOrDocumentsReader
{
    public HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords)
    {
        return invertedIndex
            .Where(x => orWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
