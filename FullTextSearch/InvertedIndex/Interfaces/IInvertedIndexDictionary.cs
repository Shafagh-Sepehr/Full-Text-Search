namespace FullTextSearch.InvertedIndex.Interfaces;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
}
