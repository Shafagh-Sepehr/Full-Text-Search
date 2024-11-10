namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface IDocumentReader
{
    HashSet<string> GetAndDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords);
    HashSet<string> GetOrDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords);
    HashSet<string> GetNotDocuments(IReadOnlyDictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords);
}
