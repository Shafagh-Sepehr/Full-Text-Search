namespace FullTextSearch.Application.DocumentsReader;

internal interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords);
}
