using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class OrDocumentsReader(Dictionary<string, List<string>> invertedIndex, List<string> orWords) : IOrDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;
    private readonly List<string>                     _orWords       = orWords;

    public HashSet<string> GetOrDocuments()
    {
        return _invertedIndex
            .Where(x => _orWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
