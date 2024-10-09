namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface IOrDocumentsReader
{
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords);
}
