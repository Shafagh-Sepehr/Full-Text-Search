using Porter2Stemmer;

namespace CodeStar2;

internal class QuerySearcher(
    Dictionary<string, List<string>> _invertedIndex,
    string _query,
    IPorter2Stemmer _stemmer)
{
    private readonly Dictionary<string, List<string>> _invertedIndex = _invertedIndex;
    private readonly string[]                         _queryWords    = _query.Trim().Split();
    private readonly IPorter2Stemmer                  _stemmer       = _stemmer;

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
        else
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

    public IEnumerable<string> Search()
    {
        if(OrWords.Count>0 && AndWords.Count>0)
            return GetAndDocs().Intersect(GetOrDocs()).Except(GetNotDocs());
        else if (AndWords.Count > 0)
            return GetAndDocs().Except(GetNotDocs());
        else
            return GetOrDocs().Except(GetNotDocs());
    }
}