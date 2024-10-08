using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;

public class DocumentReader(
    IAndDocumentsReader andDocumentsReader,
    IOrDocumentsReader orDocumentsReader,
    INotDocumentsReader notDocumentsReader) : IDocumentReader
{
    private readonly IAndDocumentsReader _andDocumentsReader = andDocumentsReader;
    private readonly IOrDocumentsReader  _orDocumentsReader  = orDocumentsReader;
    private readonly INotDocumentsReader _notDocumentsReader = notDocumentsReader;


    public HashSet<string> GetAndDocuments(List<string> andWords) => _andDocumentsReader.GetAndDocuments(andWords);
    public HashSet<string> GetOrDocuments(List<string> orWords) => _orDocumentsReader.GetOrDocuments(orWords);
    public HashSet<string> GetNotDocuments(List<string> notWords) => _notDocumentsReader.GetNotDocuments(notWords);
}
