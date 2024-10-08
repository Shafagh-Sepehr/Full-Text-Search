namespace FullTextSearch.Application.DocumentsReader.Interfaces;

public interface IAndDocumentsReader
{
    HashSet<string> GetAndDocuments(List<string> andWords);
}
