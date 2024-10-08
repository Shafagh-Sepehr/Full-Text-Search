using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal class QuerySearcher(IPorter2Stemmer? stemmer = null) : IQuerySearcher
{
    private          Dictionary<string, List<string>> _invertedIndex = null!;
    private          string[]                         _queryWords    = null!;
    private readonly IPorter2Stemmer                  _stemmer       = stemmer ?? new EnglishPorter2Stemmer();


    public IEnumerable<string> Search(string query, Dictionary<string, List<string>> invertedIndex)
    {
        IEnumerable<string> result = [];
        
        if (string.IsNullOrWhiteSpace(query))
            return result;

        _queryWords = query.Trim().Split();
        _invertedIndex = invertedIndex;


        if (OrWords.Count > 0 && AndWords.Count > 0)
            result = GetDocumentsForAndQueries().Intersect(GetDocumentsForOrQueries())
                .Except(GetDocumentsForNotQueries());
        else if (AndWords.Count > 0)
            result = GetDocumentsForAndQueries().Except(GetDocumentsForNotQueries());
        else if (OrWords.Count > 0)
            result = GetDocumentsForOrQueries().Except(GetDocumentsForNotQueries());
        
        return result;
    }
    
    private List<string> AndWords
    {
        get
        {
            return _queryWords
                .Where(x => x[0] != '+' && x[0] != '-')
                .Select(x => _stemmer.Stem(x).Value)
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
                .Select(x => _stemmer.Stem(x).Value)
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
                .Select(x => _stemmer.Stem(x).Value)
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
