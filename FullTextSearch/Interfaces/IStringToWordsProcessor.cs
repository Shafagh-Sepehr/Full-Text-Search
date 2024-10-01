using Porter2Stemmer;

namespace FullTextSearch.Interfaces;

public interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source, IPorter2Stemmer stemmer);
}
