using FullTextSearch.Application.InvertedIndex;
using FullTextSearch.IO;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

internal static class Program
{
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();
    
    private static void Main()
    {
        var documentsPath = ConfigHolder.Config["DocumentsPath"];
        var bannedWords = ConfigHolder.Config.GetSection("BannedWords").Get<string[]>();
        if (documentsPath == null)
        {
            throw new ArgumentNullException(nameof(documentsPath),"can't be null");
        }
        
        IInvertedIndexFactory invertedIndexFactory = new InvertedIndexFactory();
        var invertedIndex = invertedIndexFactory.Create(documentsPath, bannedWords);
        
        Output.Write("Search: ");
        var query = Input.ReadLine();
        
        
        var result = invertedIndex.Search(query);
        
        Output.WriteLine("Result:");
        
        foreach (var doc in result) Output.WriteLine(doc);
    }
    
    
}
