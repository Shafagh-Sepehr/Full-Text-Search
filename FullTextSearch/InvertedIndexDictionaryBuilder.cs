using System.Text.Json;
using CodeStar2.Interfaces;
using Porter2Stemmer;

namespace CodeStar2;

internal class InvertedIndexDictionaryBuilder(IEnumerable<string>? banned = null) : IInvertedIndexDictionaryBuilder
{
    private readonly StringToWordsProcessor           _stringToWordsProcessor = new(banned);
    private          Dictionary<string, List<string>> _invertedIndex          = new();
    private          IPorter2Stemmer                  _stemmer                = null!;

    public Dictionary<string, List<string>> Build(string filepath, IPorter2Stemmer stemmer)
    {
        _stemmer = stemmer;

        var files = Directory.GetFiles(filepath);


        if (File.Exists("/home/shafagh/Desktop/EnglishData/inverted_index.json"))
        {
            var invertedIndexJson = File.ReadAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json");
            _invertedIndex = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(invertedIndexJson)!;
        }
        else
        {
            FillInvertedIndex(files);
            var invertedIndexJson = JsonSerializer.Serialize(_invertedIndex);
            File.WriteAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json", invertedIndexJson);
        }

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
