namespace FullTextSearch.Application.DocumentsReader;

public interface IDocumentReader
{
    HashSet<string> GetAndDocuments();
    HashSet<string> GetOrDocuments();
    HashSet<string> GetNotDocuments();
}
