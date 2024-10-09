using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;

public class OrDocumentsReader : IOrDocumentsReader
{
    public HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords)
    {
        return invertedIndex
            .Where(x => orWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
