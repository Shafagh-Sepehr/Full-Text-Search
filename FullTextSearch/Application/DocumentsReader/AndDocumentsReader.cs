namespace FullTextSearch.Application.DocumentsReader;

internal class AndDocumentsReader : IAndDocumentsReader
{
    public HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, List<string> andWords)
    {
        var andDocsList = invertedIndex
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
