namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface IOrDocumentsReader
{
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords);
}
