namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface INotDocumentsReader
{
    HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords);
}
