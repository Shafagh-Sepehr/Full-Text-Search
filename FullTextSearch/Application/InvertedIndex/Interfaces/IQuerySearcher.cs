namespace FullTextSearch.Application.InvertedIndex.Interfaces;

public interface IQuerySearcher
{
    IEnumerable<string> Search(string query);
}
