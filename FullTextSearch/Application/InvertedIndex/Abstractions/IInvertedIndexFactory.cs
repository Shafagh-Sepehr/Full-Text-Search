namespace FullTextSearch.Application.InvertedIndex.Abstractions;

public interface IInvertedIndexFactory
{
    IInvertedIndexDictionary Create(string path, IEnumerable<string>? banned);
}
