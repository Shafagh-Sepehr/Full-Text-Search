using FullTextSearch.Application.InvertedIndex.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class InvertedIndexDictionaryFiller(IStringToWordsProcessor stringToWordsProcessor)
    : IInvertedIndexDictionaryFiller
{
    private readonly Dictionary<string, List<string>> _invertedIndex    = new();
    private readonly IStringToWordsProcessor          _toWordsProcessor = stringToWordsProcessor
                                                                       ?? throw new ArgumentNullException(nameof(stringToWordsProcessor));

    public IReadOnlyDictionary<string, List<string>> Build(string filepath)
    {
        var files = Directory.GetFiles(filepath);

        FillInvertedIndexFromFile(files);

        return _invertedIndex;
    }

    public void Construct(IReadOnlyList<string>? bannedWords)
    {
        _toWordsProcessor.Construct(bannedWords);
    }

    private void FillInvertedIndexFromFile(string[] files)
    {
        foreach (var fileName in files)
        {
            var content = File.ReadAllText(fileName);

            var words = _toWordsProcessor.TrimSplitAndStemString(content);

            AddWordsToInvertedIndex(words, fileName);
        }
    }

    private void AddWordsToInvertedIndex(IEnumerable<string> words, string fileName)
    {
        foreach (var word in words) CreateOrUpdateValue(word, fileName);
    }

    private void CreateOrUpdateValue(string word, string fileName)
    {
        if (_invertedIndex.TryGetValue(word, out var value))
            value.Add(fileName.Split('/')[^1]);
        else
            _invertedIndex[word] = [fileName.Split('/')[^1],];
    }
}
