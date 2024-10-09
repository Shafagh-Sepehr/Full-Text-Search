using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal class InvertedIndexDictionaryFiller(IStringToWordsProcessor stringToWordsProcessor)
    : IInvertedIndexDictionaryFiller
{
    private readonly IStringToWordsProcessor          _stringToWordsProcessor = stringToWordsProcessor;
    private readonly Dictionary<string, List<string>> _invertedIndex          = new();

    public Dictionary<string, List<string>> Build(string filepath)
    {
        string[] files = Directory.GetFiles(filepath);
        
        FillInvertedIndexFromFile(files);
        
        return _invertedIndex;
    }
    
    public void Construct(IEnumerable<string>? banned)
    {
        _stringToWordsProcessor.Construct(banned);
    }

    private void FillInvertedIndexFromFile(string[] files)
    {
        foreach (string fileName in files)
        {
            string content = File.ReadAllText(fileName);

            IEnumerable<string> words = _stringToWordsProcessor.TrimSplitAndStemString(content);

            AddWordsToInvertedIndex(words, fileName);
        }
    }

    private void AddWordsToInvertedIndex(IEnumerable<string> words, string fileName)
    {
        foreach (string word in words)
        {
            CreateOrUpdateValue(word, fileName);
        }
    }

    private void CreateOrUpdateValue(string word, string fileName)
    {
        if (_invertedIndex.TryGetValue(word, out List<string>? value))
            value.Add(fileName.Split('/')[^1]);
        else
            _invertedIndex[word] = [fileName.Split('/')[^1],];
    }
}