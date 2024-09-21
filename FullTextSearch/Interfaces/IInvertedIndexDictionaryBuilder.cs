using Porter2Stemmer;

namespace CodeStar2.Interfaces;

public interface IInvertedIndexDictionaryBuilder
{
    Dictionary<string, List<string>> Build(string filepath);
}
