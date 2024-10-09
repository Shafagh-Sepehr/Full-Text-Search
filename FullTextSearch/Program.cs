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
        var serviceProvider = ConfigureServices();


        var invertedIndex = serviceProvider.GetService<IInvertedIndexDictionary>();
        ArgumentNullException.ThrowIfNull(invertedIndex);
        invertedIndex.Construct(AppSettings.DocumentsPath, ["will",]);

        Output.Write("Search: ");
        string query = Input.ReadLine();


        var result = invertedIndex.Search(query);

        Output.WriteLine("Result:");

        foreach (string doc in result) Output.WriteLine(doc);
    }

    private static ServiceProvider ConfigureServices()
    {
        var serviceCollector = new ServiceCollection();

        serviceCollector.AddSingleton<IPorter2Stemmer, EnglishPorter2Stemmer>();

        serviceCollector.AddTransient<IInvertedIndexDictionary, InvertedIndexDictionary>();
        serviceCollector.AddTransient<IInvertedIndexDictionaryFiller, InvertedIndexDictionaryFiller>();
        serviceCollector.AddTransient<IQuerySearcher, QuerySearcher>();
        serviceCollector.AddTransient<IStringToWordsProcessor, StringToWordsProcessor>();

        serviceCollector.AddSingleton<IDocumentReader, DocumentReader>();
        serviceCollector.AddSingleton<IAndDocumentsReader, AndDocumentsReader>();
        serviceCollector.AddSingleton<IOrDocumentsReader, OrDocumentsReader>();
        serviceCollector.AddSingleton<INotDocumentsReader, NotDocumentsReader>();

        serviceCollector.AddSingleton<IWordsProcessor, WordsProcessor>();
        serviceCollector.AddSingleton<IAndWordsProcessor, PrefixBasedAndWordsProcessor>();
        serviceCollector.AddSingleton<IOrWordsProcessor, PrefixBasedOrWordsProcessor>();
        serviceCollector.AddSingleton<INotWordsProcessor, PrefixBasedNotWordsProcessor>();

        serviceCollector.AddSingleton<ISearcher, Searcher>();
        serviceCollector.AddSingleton<IAndOrNotSearcher, AndOrNotSearcher>();
        serviceCollector.AddSingleton<IAndNotSearcher, AndNotSearcher>();
        serviceCollector.AddSingleton<IOrNotSearcher, OrNotSearcher>();

        return serviceCollector.BuildServiceProvider();
    }
}
