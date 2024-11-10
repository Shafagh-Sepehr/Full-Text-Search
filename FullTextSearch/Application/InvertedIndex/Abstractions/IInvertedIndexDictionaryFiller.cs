namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface IInvertedIndexDictionaryFiller
{
    IReadOnlyDictionary<string, List<string>> Build(string filepath);
    public void Construct(IReadOnlyList<string>? bannedWords);
}