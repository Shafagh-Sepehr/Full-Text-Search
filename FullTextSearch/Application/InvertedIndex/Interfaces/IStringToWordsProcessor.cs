namespace FullTextSearch.Application.InvertedIndex.Interfaces;

public interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source);
}
