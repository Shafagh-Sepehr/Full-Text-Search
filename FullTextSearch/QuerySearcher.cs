using Porter2Stemmer;

namespace CodeStar2;

public class QuerySearcher(
    Dictionary<string, List<string>> _invertedIndex,
    IPorter2Stemmer _stemmer)
{
    private readonly Dictionary<string, List<string>> _invertedIndex = _invertedIndex;
    private readonly IPorter2Stemmer                  _stemmer       = _stemmer;
    private          string[]                         _queryWords    = null!;

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

    private List<string> GetAndDocs()
    {
        var andDocsList = _invertedIndex
            .Where(x => AndWords.Contains(x.Key))
            .Select(x => x.Value).ToList();

        if (andDocsList.Count > 0)
            return andDocsList.Skip(1).Aggregate(new List<string>(andDocsList[0]),
                                                 (result, list) => result.Intersect(list).ToList());
        return [];
    }

    private List<string> GetOrDocs()
    {
        return _invertedIndex
            .Where(x => OrWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x).ToList();
    }

    private List<string> GetNotDocs()
    {
        return _invertedIndex
            .Where(x => NotWords.Contains(x.Key))
            .Select(x => x.Value).SelectMany(x => x).ToList();
    }

    public IEnumerable<string> Search(string query)
    {
        _queryWords = query.Trim().Split();

        if (OrWords.Count > 0 && AndWords.Count > 0)
            return GetAndDocs().Intersect(GetOrDocs()).Except(GetNotDocs());
        else if (AndWords.Count > 0)
            return GetAndDocs().Except(GetNotDocs());
        else
            return GetOrDocs().Except(GetNotDocs());
    }
}
