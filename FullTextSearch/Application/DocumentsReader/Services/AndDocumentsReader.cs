using FullTextSearch.Application.DocumentsReader.Abstractions;

namespace FullTextSearch.Application.DocumentsReader.Services;

internal sealed class AndDocumentsReader : IAndDocumentsReader
{
    public HashSet<string> GetAndDocuments(Dictionary<string, List<string>> invertedIndex, IReadOnlyList<string> andWords)
    {
        var allAndWordsExist = andWords.All(invertedIndex.ContainsKey);
        if (!allAndWordsExist) return [];
        
        
        var andDocsList = invertedIndex
            .Where(x => andWords.Contains(x.Key))
            .Select(x => x.Value).ToList();

        if (andDocsList.Count < 1) return [];

        var result = IntersectAllElements(andDocsList);
        return result;
    }

    private static HashSet<string> IntersectAllElements(List<List<string>> andDocsList)
    {
        var result = new HashSet<string>(andDocsList[0]);
        
        foreach (var docList in andDocsList.Skip(1))
        {
            result.IntersectWith(docList);
        }

        return result;
    }
}
