using Porter2Stemmer;

namespace CodeStar2;

internal static class Program
{
    private static void Main()
    {
        
        var invertedIndex = new InvertedIndexDictionary("/home/shafagh/Desktop/EnglishData", ["will",]);


        Console.Write("Search: ");
        string? query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) return;

        IEnumerable<string> result = invertedIndex.Search(query);

        Console.WriteLine("Result:");
        foreach (string doc in result)
            Console.WriteLine(doc);
    }
}
