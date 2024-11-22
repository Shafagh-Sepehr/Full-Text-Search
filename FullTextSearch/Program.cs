using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.IO.Abstractions;
using FullTextSearch.IO.Services;

namespace FullTextSearch;

internal static class Program
{
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();
    
    private static void Main()
    {

        IInvertedIndexFactory invertedIndexFactory = new InvertedIndexFactory();
        var invertedIndex = invertedIndexFactory.Create();
        
        Output.Write("Search: ");
        var query = Input.ReadLine();
        
        
        var result = invertedIndex.Search(query);
        
        Output.WriteLine("Result:");
        
        foreach (var doc in result) Output.WriteLine(doc);
    }
}
