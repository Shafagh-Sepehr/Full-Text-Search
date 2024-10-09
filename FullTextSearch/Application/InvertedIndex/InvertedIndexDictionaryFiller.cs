namespace FullTextSearch.Application.InvertedIndex;

internal class InvertedIndexDictionaryFiller(IStringToWordsProcessor stringToWordsProcessor)
    : IInvertedIndexDictionaryFiller
{
    private IStringToWordsProcessor          ToWordsProcessor { get; } = stringToWordsProcessor;
    private Dictionary<string, List<string>> InvertedIndex    { get; } = new();

    public Dictionary<string, List<string>> Build(string filepath)
    {
        string[] files = Directory.GetFiles(filepath);

        FillInvertedIndexFromFile(files);

        return InvertedIndex;
    }

    public void Construct(IEnumerable<string>? banned)
    {
        ToWordsProcessor.Construct(banned);
    }

    private void FillInvertedIndexFromFile(string[] files)
    {
        foreach (string fileName in files)
        {
            string content = File.ReadAllText(fileName);

            var words = ToWordsProcessor.TrimSplitAndStemString(content);

            AddWordsToInvertedIndex(words, fileName);
        }
    }

    private void AddWordsToInvertedIndex(IEnumerable<string> words, string fileName)
    {
        foreach (string word in words) CreateOrUpdateValue(word, fileName);
    }

    private void CreateOrUpdateValue(string word, string fileName)
    {
        if (InvertedIndex.TryGetValue(word, out var value))
            value.Add(fileName.Split('/')[^1]);
        else
            InvertedIndex[word] = [fileName.Split('/')[^1],];
    }
}
