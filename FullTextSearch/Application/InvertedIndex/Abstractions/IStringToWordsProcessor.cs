namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source);
    public void Construct(IReadOnlyList<string>? bannedWords);
}