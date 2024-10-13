namespace FullTextSearch.Application.DocumentsReader;

internal interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords);
}
