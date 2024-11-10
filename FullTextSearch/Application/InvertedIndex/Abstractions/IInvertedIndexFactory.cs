namespace FullTextSearch.Application.InvertedIndex.Abstractions;

public interface IInvertedIndexFactory
{
    IInvertedIndexDictionary Create(string path, IReadOnlyList<string>? bannedWords);
}
