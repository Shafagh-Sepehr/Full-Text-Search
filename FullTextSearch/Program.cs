using FullTextSearch.Exceptions;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

internal static class Program
{
    
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();
    
    private static void Main()
    {
        
        if (Configuration["documents_path"] == null)
            throw new Exception("documents_path is not set in appsetings.json");
        var invertedIndex = new InvertedIndexDictionary(Configuration["documents_path"]!, ["will",]);
        
        
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
