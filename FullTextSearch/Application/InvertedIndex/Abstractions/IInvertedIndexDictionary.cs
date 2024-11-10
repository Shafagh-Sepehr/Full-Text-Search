namespace FullTextSearch.Application.InvertedIndex.Abstractions;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
    public void Construct(string path, IReadOnlyList<string>? bannedWords);
}
