using FullTextSearch.Application.DocumentsReader.Abstractions;

namespace FullTextSearch.Application.DocumentsReader.Services;

internal sealed class DocumentReader(
    IAndDocumentsReader andDocumentsReader,
    IOrDocumentsReader orDocumentsReader,
    INotDocumentsReader notDocumentsReader)
    : IDocumentReader
{
    private readonly IAndDocumentsReader _andDocumentsReader = andDocumentsReader ?? throw new ArgumentNullException(nameof(andDocumentsReader));
    private readonly INotDocumentsReader _notDocumentsReader = notDocumentsReader ?? throw new ArgumentNullException(nameof(notDocumentsReader));
    private readonly IOrDocumentsReader  _orDocumentsReader  = orDocumentsReader ?? throw new ArgumentNullException(nameof(orDocumentsReader));


    public HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords)
        => _andDocumentsReader.GetAndDocuments(invertedIndex, andWords);

    public HashSet<string> GetOrDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> orWords) =>
        _orDocumentsReader.GetOrDocuments(invertedIndex, orWords);

    public HashSet<string> GetNotDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> notWords) =>
        _notDocumentsReader.GetNotDocuments(invertedIndex, notWords);
}
