using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class AndDocumentsReader(Dictionary<string, List<string>> invertedIndex, List<string> andWords) : IAndDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;
    private readonly List<string>                     _andWords      = andWords;

    public HashSet<string> GetAndDocuments()
    {
        List<List<string>> andDocsList = _invertedIndex
            .Where(x => _andWords.Contains(x.Key))
            .Select(x => x.Value).ToList();

        if (andDocsList.Count < 1) return [];
            
        var result = new HashSet<string>(andDocsList[0]);
        
        for (var i = 1; i < andDocsList.Count; i++)
        {
            result.IntersectWith(andDocsList[i]);
        }

        return result;
    }
}
