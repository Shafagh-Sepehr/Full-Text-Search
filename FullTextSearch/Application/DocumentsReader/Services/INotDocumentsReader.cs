namespace FullTextSearch.Application.DocumentsReader;

internal interface INotDocumentsReader
{
    HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords);
}
