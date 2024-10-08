namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface IDocumentReader
{
    HashSet<string> GetAndDocuments(List<string> andWords);
    HashSet<string> GetOrDocuments(List<string> orWords);
    HashSet<string> GetNotDocuments(List<string> notWords);
}
