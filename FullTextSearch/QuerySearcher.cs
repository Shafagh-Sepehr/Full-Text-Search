using Porter2Stemmer;

namespace CodeStar2;

public static class QuerySearcher
{
    private static Dictionary<string, List<string>> _invertedIndex = null!;
    private static IPorter2Stemmer                  _stemmer       = null!;
    private static string[]                         _queryWords    = null!;

    private static List<string> AndWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] != '+' && x[0] != '-')
                .Select(x => _stemmer.Stem(x).Value)
                .ToList();
        }
    }

    private static List<string> OrWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] == '+')
                .Select(x => x.Substring(1, x.Length - 1))
                .Select(x => _stemmer.Stem(x).Value)
                .ToList();
        }
    }

    private static List<string> NotWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] == '-')
                .Select(x => x.Substring(1, x.Length - 1))
                .Select(x => _stemmer.Stem(x).Value)
                .ToList();
        }
    }

    private static HashSet<string> GetDocumentsForAndQueries()
    {
        var andDocsList = _invertedIndex
            .Where(x => AndWords.Contains(x.Key))
            .Select(x => x.Value).ToList();
        
        
        var result = new HashSet<string>(andDocsList[0]);
        for (var i = 1; i < andDocsList.Count; i++)
            result.IntersectWith(andDocsList[i]);
        
        return result;
        
    }

    private static HashSet<string> GetDocumentsForOrQueries()
    {
        return _invertedIndex
            .Where(x => OrWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }

    private static HashSet<string> GetDocumentsForNotQueries()
    {
        return _invertedIndex
            .Where(x => NotWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x)
            .ToHashSet();
    }

    public static IEnumerable<string> Search(string query, Dictionary<string, List<string>> invertedIndex, IPorter2Stemmer stemmer)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];
        
        _queryWords = query.Trim().Split();
        _invertedIndex = invertedIndex;
        _stemmer = stemmer;

        if (OrWords.Count > 0 && AndWords.Count > 0)
            return GetDocumentsForAndQueries().Intersect(GetDocumentsForOrQueries()).Except(GetDocumentsForNotQueries());
        else if (AndWords.Count > 0)
            return GetDocumentsForAndQueries().Except(GetDocumentsForNotQueries());
        else if (OrWords.Count > 0)
            return GetDocumentsForOrQueries().Except(GetDocumentsForNotQueries());
        else
            return [];
    }
}
