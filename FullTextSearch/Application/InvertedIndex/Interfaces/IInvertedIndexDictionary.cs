namespace FullTextSearch.Application.InvertedIndex.Interfaces;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
}
