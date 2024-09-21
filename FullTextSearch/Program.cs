using Porter2Stemmer;

namespace CodeStar2;

internal static class Program
{
    private static void Main()
    {
        IPorter2Stemmer stemmer = new EnglishPorter2Stemmer();
        
        var invertedIndex = new InvertedIndexDictionary("/home/shafagh/Desktop/EnglishData",
                                                        new InvertedIndexDictionaryBuilder(new StringToWordsProcessor(["will",]), stemmer),
                                                        new QuerySearcher(stemmer));


        Console.Write("Search: ");
        var query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) return;

        IEnumerable<string> result = invertedIndex.Search(query);

        Console.WriteLine("Result:");
        foreach (var doc in result)
            Console.WriteLine(doc);
    }
}
