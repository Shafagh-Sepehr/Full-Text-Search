namespace FullTextSearch.Application.InvertedIndex.Abstractions;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
}
