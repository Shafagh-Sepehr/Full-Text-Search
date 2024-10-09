namespace FullTextSearch.Application.DocumentsReader;

public interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords);
}
