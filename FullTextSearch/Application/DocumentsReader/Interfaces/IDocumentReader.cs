namespace FullTextSearch.Application.DocumentsReader;

public interface IDocumentReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords);
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords);
    HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords);
}
