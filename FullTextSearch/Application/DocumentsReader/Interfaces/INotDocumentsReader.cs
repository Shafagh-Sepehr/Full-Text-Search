namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface INotDocumentsReader
{
    HashSet<string> GetNotDocuments(List<string> notWords);
}
