namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface IOrDocumentsReader
{
    HashSet<string> GetOrDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords);
}
