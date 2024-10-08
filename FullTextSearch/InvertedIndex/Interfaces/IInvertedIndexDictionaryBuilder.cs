namespace FullTextSearch.InvertedIndex.Interfaces;

public interface IInvertedIndexDictionaryBuilder
{
    Dictionary<string, List<string>> Build(string filepath);
}
