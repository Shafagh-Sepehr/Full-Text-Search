namespace FullTextSearch.Application.DocumentsReader;

public class NotDocumentsReader : INotDocumentsReader
{
    public HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords)
    {
        return invertedIndex
            .Where(x => notWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
