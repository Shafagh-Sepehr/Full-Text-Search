using System.Text.Json;
using Porter2Stemmer;

namespace CodeStar2;

internal static class InvertedIndexDictionaryBuilder
{
    private static IPorter2Stemmer        _stemmer                = null!;
    private static StringToWordsProcessor _stringToWordsProcessor = null!;


    public static Dictionary<string, List<string>> Build(string filepath, IPorter2Stemmer stemmer,
                                                         IEnumerable<string>? banned = null)
    {
        Dictionary<string, List<string>> invertedIndex = new();
        _stemmer = stemmer;

        _stringToWordsProcessor = new StringToWordsProcessor(banned);
        string[] files = Directory.GetFiles(filepath);

        
        if (File.Exists("/home/shafagh/Desktop/EnglishData/inverted_index.json"))
        {
            var invertedIndexJson = File.ReadAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json");
            invertedIndex = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(invertedIndexJson)!;
        }
        else
        {
            FillInvertedIndex(invertedIndex, files);
            string invertedIndexJson = JsonSerializer.Serialize(invertedIndex);
            File.WriteAllText("/home/shafagh/Desktop/EnglishData/inverted_index.json", invertedIndexJson);
        }

        return invertedIndex;
    }

    private static void FillInvertedIndex(Dictionary<string, List<string>> invertedIndex, string[] files)
    {
        foreach (var fileName in files)
        {
            var content = File.ReadAllText(fileName);

            var words = _stringToWordsProcessor.TrimSplitAndStemString(content, _stemmer);

            foreach (var word in words)
                CreateOrUpdateValue(invertedIndex, word, fileName);
        }
    }

    private static void CreateOrUpdateValue(Dictionary<string, List<string>> invertedIndex, string word,
                                            string fileName)
    {
        if (invertedIndex.TryGetValue(word, out var value))
            value.Add(fileName.Split('/')[^1]);
        else
            invertedIndex[word] = [fileName.Split('/')[^1],];
    }
}
