using System.Collections.Concurrent;

namespace CodeStar2;

internal static class Program
{
    private static void Main()
    {
        var invertedIndex = new InvertedIndexDictionary("/home/shafagh/Desktop/EnglishData",["will"]);
        
        
        Console.Write("Search: ");
        var query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) return;
        
        var result = invertedIndex.Search(query);
        
        Console.WriteLine("Result:");
        foreach (var doc in result)
            Console.WriteLine(doc);
    }
}
