using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.InvertedIndex;
using FullTextSearch.Application.Searchers;
using FullTextSearch.Application.WordsProcessors;
using FullTextSearch.IO;
using Microsoft.Extensions.DependencyInjection;
using Porter2Stemmer;

namespace FullTextSearch;

internal static class Program
{
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();

    private static void Main()
    {
        
        IInvertedIndexFactory invertedIndexFactory = new InvertedIndexFactory();
        var invertedIndex = invertedIndexFactory.Create(AppSettings.DocumentsPath, ["will",]);
        
        Output.Write("Search: ");
        var query = Input.ReadLine();


        var result = invertedIndex.Search(query);

        Output.WriteLine("Result:");

        foreach (var doc in result) Output.WriteLine(doc);
    }

    
}
