namespace FullTextSearch.Application.InvertedIndex;

public interface IInvertedIndexDictionaryFiller
{
    Dictionary<string, List<string>> Build(string filepath);
    public void Construct(IEnumerable<string>? banned);
}
