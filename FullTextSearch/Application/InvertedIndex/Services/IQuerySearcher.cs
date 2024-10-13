namespace FullTextSearch.Application.InvertedIndex;

internal interface IQuerySearcher
{
    IEnumerable<string> Search(string query);
    public void Construct(Dictionary<string, List<string>> invertedIndex);
}
