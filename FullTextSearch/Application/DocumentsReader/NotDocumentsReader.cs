using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class NotDocumentsReader(Dictionary<string, List<string>> invertedIndex) : INotDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;

    public HashSet<string> GetNotDocuments(List<string> notWords)
    {
        return _invertedIndex
            .Where(x => notWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
