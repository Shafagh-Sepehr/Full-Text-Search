namespace FullTextSearch.Application.InvertedIndex;

internal interface IInvertedIndexDictionaryFiller
{
    Dictionary<string, List<string>> Build(string filepath);
    public void Construct(IEnumerable<string>? banned);
}
