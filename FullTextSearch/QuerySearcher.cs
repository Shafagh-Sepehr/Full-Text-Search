using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

public class QuerySearcher(IPorter2Stemmer stemmer) : IQuerySearcher
{
    private Dictionary<string, List<string>> _invertedIndex = null!;
    private string[]                         _queryWords    = null!;
    
    
    public IEnumerable<string> Search(string query, Dictionary<string, List<string>> invertedIndex)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        _queryWords = query.Trim().Split();
        _invertedIndex = invertedIndex;


        if (OrWords.Count > 0 && AndWords.Count > 0)
            return GetDocumentsForAndQueries().Intersect(GetDocumentsForOrQueries())
                .Except(GetDocumentsForNotQueries());
        if (AndWords.Count > 0)
            return GetDocumentsForAndQueries().Except(GetDocumentsForNotQueries());
        if (OrWords.Count > 0)
            return GetDocumentsForOrQueries().Except(GetDocumentsForNotQueries());
        return [];
    }
    
    private List<string> AndWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] != '+' && x[0] != '-')
                .Select(x => stemmer.Stem(x).Value)
                .ToList();
        }
    }

    private List<string> OrWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] == '+')
                .Select(x => x.Substring(1, x.Length - 1))
                .Select(x => stemmer.Stem(x).Value)
                .ToList();
        }
    }

    private List<string> NotWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] == '-')
                .Select(x => x.Substring(1, x.Length - 1))
                .Select(x => stemmer.Stem(x).Value)
                .ToList();
        }
    }


    private HashSet<string> GetDocumentsForAndQueries()
    {
        List<List<string>> andDocsList = _invertedIndex
            .Where(x => AndWords.Contains(x.Key))
            .Select(x => x.Value).ToList();

        if (andDocsList.Count < 1) return [];
            
        var result = new HashSet<string>(andDocsList[0]);
        for (var i = 1; i < andDocsList.Count; i++)
            result.IntersectWith(andDocsList[i]);

        return result;
    }

    private HashSet<string> GetDocumentsForOrQueries()
    {
        return _invertedIndex
            .Where(x => OrWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }

    private HashSet<string> GetDocumentsForNotQueries()
    {
        return _invertedIndex
            .Where(x => NotWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }
}
