using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.DocumentsReader.Services;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Application.Searchers.Services;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Application.WordsProcessors.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Porter2Stemmer;

namespace FullTextSearch;

internal static class Services
{
    private static ServiceProvider? _serviceProvider;

    public static ServiceProvider ServiceProvider => _serviceProvider ??= ConfigureServices();

    private static ServiceProvider ConfigureServices()
    {
        var serviceCollector = new ServiceCollection();

        serviceCollector.AddSingleton<IPorter2Stemmer, EnglishPorter2Stemmer>();

        serviceCollector.AddTransient<IInvertedIndexDictionary, InvertedIndexDictionary>();
        serviceCollector.AddTransient<IInvertedIndexDictionaryFiller, InvertedIndexDictionaryFiller>();
        serviceCollector.AddTransient<IQuerySearcher, QuerySearcher>();
        serviceCollector.AddTransient<IStringToWordsProcessor, StringToWordsProcessor>();
        serviceCollector.AddSingleton<IInvertedIndexFactory, InvertedIndexFactory>();

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

        serviceCollector.AddTransient<IConfigurationBuilder, ConfigurationBuilder>();
        serviceCollector.AddTransient<IConfigurationService, ConfigurationServiceService>();
        
        return serviceCollector.BuildServiceProvider();
    }
}
