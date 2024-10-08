using FullTextSearch.Application.DocumentsReader.Interfaces;

namespace FullTextSearch.Application.DocumentsReader;



public class AndDocumentsReader(Dictionary<string, List<string>> invertedIndex) : IAndDocumentsReader
{
    private readonly Dictionary<string, List<string>> _invertedIndex = invertedIndex;

    public HashSet<string> GetAndDocuments(List<string> andWords)
    {
        List<List<string>> andDocsList = _invertedIndex
            .Where(x => andWords.Contains(x.Key))
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
