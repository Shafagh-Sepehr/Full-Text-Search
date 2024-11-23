using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.DocumentsReader.Services;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.RegexCheckers.Services;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Application.Searchers.Services;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Services;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Services;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Services;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Services;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Services;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Application.WordsProcessors.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Porter2Stemmer;

namespace FullTextSearch;

internal static class ServiceCollection
{
    private static ServiceProvider? _serviceProvider;
    
    public static ServiceProvider ServiceProvider => _serviceProvider ??= ConfigureServices();
    
    private static ServiceProvider ConfigureServices()
    {
        var serviceCollector = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        
        serviceCollector.AddSingleton<IPorter2Stemmer, EnglishPorter2Stemmer>();
        
        serviceCollector.AddTransient<IInvertedIndexDictionary, InvertedIndexDictionary>();
        serviceCollector.AddTransient<IInvertedIndexDictionaryFiller, InvertedIndexDictionaryFiller>();
        serviceCollector.AddTransient<IQuerySearcher, QuerySearcher>();
        serviceCollector.AddTransient<IStringToWordsProcessor, StringToWordsProcessor>();
        serviceCollector.AddSingleton<IInvertedIndexFactory, InvertedIndexFactory>();
        serviceCollector.AddSingleton<ISearchExecutor, SearchExecutor>();
        
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
        
        serviceCollector.AddSingleton<IRegexChecker, RegexChecker>();
        serviceCollector.AddSingleton<IUrlRegexChecker, UrlRegexChecker>();
        serviceCollector.AddSingleton<IEmailRegexChecker, EmailRegexChecker>();
        serviceCollector.AddSingleton<IPhoneNumberRegexChecker, PhoneNumberRegexChecker>();
        
        serviceCollector.AddSingleton<IStringListNoiseCleaner, StringListNoiseCleaner>();
        serviceCollector.AddSingleton<IStringTrimAndSplitter, StringTrimAndSplitter>();
        serviceCollector.AddSingleton<IStringListCleaner, StringListCleaner>();
        serviceCollector.AddSingleton<IStringListStemmer, StringListStemmer>();
        serviceCollector.AddSingleton<IStringListNonValidWordCleaner, StringListNonValidWordCleaner>();

        serviceCollector.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("inverted_index_appsettings.json", optional: false, reloadOnChange: true)
            .Build());

        serviceCollector.AddSingleton<IAppSettings>(sp =>
        {
            var appSettings = sp.GetRequiredService<IConfiguration>().GetSection("AppSettings").Get<AppSettings>();
            ArgumentNullException.ThrowIfNull(appSettings);
            return appSettings;
        });

        
        return serviceCollector.BuildServiceProvider();
    }
}