namespace FullTextSearch.Application.InvertedIndex;

public interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source);
    public void Construct(IEnumerable<string>? banned);
}
