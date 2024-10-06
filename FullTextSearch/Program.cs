using FullTextSearch.Exceptions;
using Microsoft.Extensions.Configuration;

namespace FullTextSearch;

internal static class Program
{
    
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    
    private static void Main()
    {

        if (Configuration["documents_path"] == null)
            throw new Exception("documents_path is not set in appsetings.json");
        
        var invertedIndex = new InvertedIndexDictionary(Configuration["documents_path"]!, ["will",]);


        Console.Write("Search: ");
        string? query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) throw new NullInputException();

        IEnumerable<string> result = invertedIndex.Search(query);

        Console.WriteLine("Result:");
        foreach (string doc in result)
            Console.WriteLine(doc);
    }
}
