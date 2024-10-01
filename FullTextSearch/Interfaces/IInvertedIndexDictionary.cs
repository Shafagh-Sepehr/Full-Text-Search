namespace FullTextSearch.Interfaces;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
}
