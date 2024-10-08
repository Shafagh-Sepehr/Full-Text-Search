using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class OrDocumentsReader(Dictionary<string, List<string>> invertedIndex) : IOrDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;

    public HashSet<string> GetOrDocuments(List<string> orWords)
    {
        return _invertedIndex
            .Where(x => orWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
