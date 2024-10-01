namespace CodeStar2.Interfaces;

public interface IQuerySearcher
{
    IEnumerable<string> Search(string query, Dictionary<string, List<string>> invertedIndex);
}
