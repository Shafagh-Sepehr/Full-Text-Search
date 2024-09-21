namespace CodeStar2;

internal static class Program
{
    private static void Main()
    {
        var invertedIndex = new InvertedIndexDictionary("/home/shafagh/Desktop/EnglishData",
                                                        new InvertedIndexDictionaryBuilder(["will",]),
                                                        new QuerySearcher());


        Console.Write("Search: ");
        var query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) return;

        IEnumerable<string> result = invertedIndex.Search(query);

        Console.WriteLine("Result:");
        foreach (var doc in result)
            Console.WriteLine(doc);
    }
}
