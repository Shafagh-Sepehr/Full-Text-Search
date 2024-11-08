namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords);
}
