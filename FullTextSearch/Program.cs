using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.ConfigurationService.Abstractions;
using FullTextSearch.IO.Abstractions;
using FullTextSearch.IO.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch;

internal static class Program
{
    private static readonly IOutput Output = new ConsoleOutput();
    private static readonly IInput  Input  = new ConsoleInput();
    
    private static void Main()
    {
        var serviceProvider = ServiceCollection.ServiceProvider;
        var config = serviceProvider.GetService<IConfigurationService>();

        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }
        
        var documentsPath = config.GetConfig()["DocumentsPath"];
        var bannedWords = config.GetConfig().GetSection("BannedWords").Get<string[]>();
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
