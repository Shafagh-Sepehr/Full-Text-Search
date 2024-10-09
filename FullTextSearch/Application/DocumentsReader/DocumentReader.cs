using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;

public class DocumentReader(
    IAndDocumentsReader andDocumentsReader,
    IOrDocumentsReader orDocumentsReader,
    INotDocumentsReader notDocumentsReader)
    : IDocumentReader
{
    private IAndDocumentsReader AndDocumentsReader { get; } = andDocumentsReader;
    private INotDocumentsReader NotDocumentsReader { get; } = notDocumentsReader;
    private IOrDocumentsReader  OrDocumentsReader  { get; } = orDocumentsReader;


    public HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords)
        => AndDocumentsReader.GetAndDocuments(invertedIndex, andWords);

    public HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords) =>
        OrDocumentsReader.GetOrDocuments(invertedIndex, orWords);

    public HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords) =>
        NotDocumentsReader.GetNotDocuments(invertedIndex, notWords);
}
