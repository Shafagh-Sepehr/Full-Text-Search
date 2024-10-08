using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

public static class InvertedIndexBuilder
{
    public static IInvertedIndexDictionary CreateFromScratch(string filepath, IEnumerable<string>? banned)
    {
        IPorter2Stemmer stemmer = new EnglishPorter2Stemmer();
        var stringToWordsProcessor = new StringToWordsProcessor(banned, stemmer);
        IInvertedIndexDictionaryBuilder invertedIndexDictionaryBuilder = new InvertedIndexDictionaryFiller(stringToWordsProcessor);
        
        
        Dictionary<string, List<string>> invertedIndex = invertedIndexDictionaryBuilder.Build(filepath);
        
        IQuerySearcher querySearcher = new QuerySearcher(invertedIndex);

        return new InvertedIndexDictionary(querySearcher);
    }

    public static IInvertedIndexDictionary Create(IQuerySearcher querySearcher) => new InvertedIndexDictionary(querySearcher);
}
