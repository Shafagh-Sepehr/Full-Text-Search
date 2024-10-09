namespace FullTextSearch.Application.InvertedIndex.Interfaces;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
    public void Construct(string path, IEnumerable<string>? banned);
}
