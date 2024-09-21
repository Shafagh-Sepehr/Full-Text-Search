namespace CodeStar2;

internal interface IInvertedIndexDictionaryBuilder
{
    Dictionary<string, List<string>> Build(string filepath);
}
