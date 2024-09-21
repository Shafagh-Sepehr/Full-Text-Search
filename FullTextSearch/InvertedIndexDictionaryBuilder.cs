using System.Text.Json;
using Porter2Stemmer;

namespace CodeStar2;

internal class InvertedIndexDictionaryBuilder(IPorter2Stemmer stemmer, IEnumerable<string>? banned = null)
{
    private readonly StringToWordsProcessor           _stringToWordsProcessor = new(banned);
    private          Dictionary<string, List<string>> _invertedIndex          = new();

    public Dictionary<string, List<string>> Build(string filepath)
    {

        
        string[] files = Directory.GetFiles(filepath);

        
        if (File.Exists("/home/shafagh/Desktop/EnglishData/inverted_index.json"))
        {
            var invertedIndexJson = File.ReadAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json");
            _invertedIndex = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(invertedIndexJson)!;
        }
        else
        {
            FillInvertedIndex(files);
            string invertedIndexJson = JsonSerializer.Serialize(_invertedIndex);
            File.WriteAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json", invertedIndexJson);
        }

        return _invertedIndex;
    }

    private void FillInvertedIndex(string[] files)
    {
        foreach (var fileName in files)
        {
            var content = File.ReadAllText(fileName);

            var words = _stringToWordsProcessor.TrimSplitAndStemString(content, stemmer);

            foreach (var word in words)
                CreateOrUpdateValue(word, fileName);
        }
    }

    private void CreateOrUpdateValue( string word, string fileName)
    {
        if (_invertedIndex.TryGetValue(word, out var value))
            value.Add(fileName.Split('/')[^1]);
        else
            _invertedIndex[word] = [fileName.Split('/')[^1],];
    }
}
