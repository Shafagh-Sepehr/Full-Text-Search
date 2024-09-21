using Porter2Stemmer;

namespace CodeStar2;

public interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source, IPorter2Stemmer stemmer);
}
