namespace FullTextSearch.Application.DocumentsReader;

public interface IOrDocumentsReader
{
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords);
}
