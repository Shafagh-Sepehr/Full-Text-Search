namespace FullTextSearch.Application.DocumentsReader;

internal interface IOrDocumentsReader
{
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords);
}
