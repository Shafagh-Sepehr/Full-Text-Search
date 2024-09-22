using System.Text.Json;
using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

public class InvertedIndexDictionaryBuilder(IStringToWordsProcessor stringToWordsProcessor, IPorter2Stemmer stemmer) : IInvertedIndexDictionaryBuilder
{
    private readonly IStringToWordsProcessor          _stringToWordsProcessor = stringToWordsProcessor;
    private          Dictionary<string, List<string>> _invertedIndex          = new();
    private readonly IPorter2Stemmer                  _stemmer                = stemmer;

    public Dictionary<string, List<string>> Build(string filepath)
    {
        var files = Directory.GetFiles(filepath);
        
        FillInvertedIndex(files);
        
        return _invertedIndex;
    }

    private void FillInvertedIndex(string[] files)
    {
        foreach (var fileName in files)
        {
            var content = File.ReadAllText(fileName);

            IEnumerable<string> words = _stringToWordsProcessor.TrimSplitAndStemString(content, _stemmer);

            foreach (var word in words)
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
