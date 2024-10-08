using FullTextSearch.Application.InvertedIndex.Interfaces;

namespace FullTextSearch.Application.InvertedIndex;

internal class InvertedIndexDictionaryBuilder(IStringToWordsProcessor stringToWordsProcessor) : IInvertedIndexDictionaryBuilder
{
    private readonly IStringToWordsProcessor          _stringToWordsProcessor = stringToWordsProcessor;
    private readonly Dictionary<string, List<string>> _invertedIndex          = new();

    public Dictionary<string, List<string>> Build(string filepath)
    {
        string[] files = Directory.GetFiles(filepath);
        
        FillInvertedIndexFromFile(files);
        
        return _invertedIndex;
    }

    private void FillInvertedIndexFromFile(string[] files)
    {
        foreach (string fileName in files)
        {
            string content = File.ReadAllText(fileName);

            IEnumerable<string> words = _stringToWordsProcessor.TrimSplitAndStemString(content);

            foreach (string word in words)
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
