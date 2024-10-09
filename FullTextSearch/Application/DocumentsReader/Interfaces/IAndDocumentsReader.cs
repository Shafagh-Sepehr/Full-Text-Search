namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords);
}
