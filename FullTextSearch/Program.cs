
using FullTextSearch.Application.InvertedIndex;
using FullTextSearch.IO;
using FullTextSearch.IO.Interfaces;

namespace FullTextSearch;

internal static class Program
{
    
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();
    
    private static void Main()
    {

        var invertedIndex = InvertedIndexBuilder.CreateFromScratch(AppSettings.DocumentsPath, ["will",]);
        
        Output.Write("Search: ");
        string query = Input.ReadLine();

        
        IEnumerable<string> result = invertedIndex.Search(query);
        
        Output.WriteLine("Result:");
        
        foreach (string doc in result)
        {
            Output.WriteLine(doc);
        }
    }
}
