using System.Text.Json;
using Porter2Stemmer;

namespace CodeStar2;

internal class InvertedIndexDictionary
{
    
    private readonly Dictionary<string, List<string>> _invertedIndex = new();
    private readonly IPorter2Stemmer            _stemmer       = new EnglishPorter2Stemmer();
    private readonly StringToWordsProcessor                    _toWords;
    

    public InvertedIndexDictionary(string filepath, List<string>? banned = null)
    {
        _toWords = new StringToWordsProcessor(banned);

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
    }

    private void FillInvertedIndex(string[] files)
    {
        foreach (var fileName in files)
        {
            var content = File.ReadAllText(fileName);

            var words = _toWords.TrimSplitAndStemString(content, _stemmer);

            foreach (var word in words)
                CreateOrUpdateValue(word, fileName);
        }
    }

    private void CreateOrUpdateValue(string word, string fileName)
    {
        if (_invertedIndex.TryGetValue(word, out var value))
            value.Add(fileName.Split('/')[^1]);
        else
            _invertedIndex[word] = [fileName.Split('/')[^1],];
    }

    
    public IEnumerable<string> Search(string query)
    {
        var searcher = new QuerySearcher(_invertedIndex, _stemmer);
        return searcher.Search(query);
    }
    
}