namespace FullTextSearch.Application.DocumentsReader;

internal interface INotDocumentsReader
{
    HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords);
}
