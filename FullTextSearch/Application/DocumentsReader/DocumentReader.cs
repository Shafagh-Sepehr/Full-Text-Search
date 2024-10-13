namespace FullTextSearch.Application.DocumentsReader;

internal class DocumentReader(
    IAndDocumentsReader andDocumentsReader,
    IOrDocumentsReader orDocumentsReader,
    INotDocumentsReader notDocumentsReader)
    : IDocumentReader
{
    private readonly IAndDocumentsReader _andDocumentsReader = andDocumentsReader;
    private readonly INotDocumentsReader _notDocumentsReader = notDocumentsReader;
    private readonly IOrDocumentsReader  _orDocumentsReader  = orDocumentsReader;


    public HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords)
        => _andDocumentsReader.GetAndDocuments(invertedIndex, andWords);

    public HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, List<string> orWords) =>
        _orDocumentsReader.GetOrDocuments(invertedIndex, orWords);

    public HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, List<string> notWords) =>
        _notDocumentsReader.GetNotDocuments(invertedIndex, notWords);
}
