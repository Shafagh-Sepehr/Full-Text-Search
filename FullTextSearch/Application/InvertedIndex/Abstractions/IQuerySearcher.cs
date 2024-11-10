namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface IQuerySearcher
{
    IReadOnlySet<string> Search(string query);
    public void Construct(IReadOnlyDictionary<string, List<string>> invertedIndex);
}
