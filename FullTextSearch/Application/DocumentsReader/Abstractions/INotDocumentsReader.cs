namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface INotDocumentsReader
{
    HashSet<string> GetNotDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords);
}
