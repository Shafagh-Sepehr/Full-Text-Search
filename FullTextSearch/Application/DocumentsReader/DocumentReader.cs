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


    public HashSet<string> GetAndDocuments() => _andDocumentsReader.GetAndDocuments();
    public HashSet<string> GetOrDocuments() => _orDocumentsReader.GetOrDocuments();
    public HashSet<string> GetNotDocuments() => _notDocumentsReader.GetNotDocuments();
}
