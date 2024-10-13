namespace FullTextSearch.Application.InvertedIndex;

internal interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source);
    public void Construct(IEnumerable<string>? banned);
}
