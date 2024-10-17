namespace FullTextSearch.Application.InvertedIndex;

public interface IInvertedIndexFactory
{
    IInvertedIndexDictionary Create(string path, IEnumerable<string>? banned);
}
