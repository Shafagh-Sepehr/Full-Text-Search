using Porter2Stemmer;

namespace CodeStar2;

public interface IInvertedIndexDictionaryBuilder
{
    Dictionary<string, List<string>> Build(string filepath, IPorter2Stemmer stemmer);
}
