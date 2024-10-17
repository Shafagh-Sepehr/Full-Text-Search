namespace FullTextSearch.Application.DocumentsReader.Abstractions;

internal interface IDocumentReader
{
    HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords);
    HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords);
    HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords);
}
