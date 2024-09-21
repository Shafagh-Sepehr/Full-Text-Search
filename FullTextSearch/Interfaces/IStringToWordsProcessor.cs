using Porter2Stemmer;

namespace CodeStar2.Interfaces;

public interface IStringToWordsProcessor
{
    IEnumerable<string> TrimSplitAndStemString(string source, IPorter2Stemmer stemmer);
}
