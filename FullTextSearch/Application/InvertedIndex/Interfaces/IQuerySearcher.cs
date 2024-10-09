namespace FullTextSearch.Application.InvertedIndex;

public interface IQuerySearcher
{
    IEnumerable<string> Search(string query);
    public void Construct(Dictionary<string, List<string>> invertedIndex);
}
