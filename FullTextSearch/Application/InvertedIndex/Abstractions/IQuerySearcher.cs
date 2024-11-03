namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface IQuerySearcher
{
    IReadOnlySet<string> Search(string query);
    public void Construct(Dictionary<string, List<string>> invertedIndex);
}
