using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class NotDocumentsReader(Dictionary<string, List<string>> invertedIndex, List<string> notWords) : INotDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;
    private readonly List<string>                     _notWords      = notWords;

    public HashSet<string> GetNotDocuments()
    {
        return _invertedIndex
            .Where(x => _notWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
