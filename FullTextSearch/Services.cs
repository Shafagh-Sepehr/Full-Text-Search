using FullTextSearch.Application.DocumentsReader;
using FullTextSearch.Application.InvertedIndex;
using FullTextSearch.Application.Searchers;
using FullTextSearch.Application.WordsProcessors;
using Microsoft.Extensions.DependencyInjection;
using Porter2Stemmer;

namespace FullTextSearch;

internal static class Services
{
    public static ServiceProvider ConfigureServices()
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

        return serviceCollector.BuildServiceProvider();
    }
}
