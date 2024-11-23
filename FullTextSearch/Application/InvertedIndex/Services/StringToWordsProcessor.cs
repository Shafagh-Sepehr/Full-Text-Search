using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class StringToWordsProcessor(
    IStringListNoiseCleaner stringListNoiseCleaner,
    IStringTrimAndSplitter stringTrimAndSplitter,
    IStringListCleaner stringListCleaner,
    IStringListStemmer stringListStemmer,
    IStringListNonValidWordCleaner stringListNonValidWordCleaner) : IStringToWordsProcessor
{
    private readonly IStringListCleaner _stringListCleaner =
        stringListCleaner ?? throw new ArgumentNullException(nameof(stringListCleaner));

    private readonly IStringListNoiseCleaner _stringListNoiseCleaner =
        stringListNoiseCleaner ?? throw new ArgumentNullException(nameof(stringListNoiseCleaner));

    private readonly IStringListNonValidWordCleaner _stringListNonValidWordCleaner =
        stringListNonValidWordCleaner ?? throw new ArgumentNullException(nameof(stringListNonValidWordCleaner));
    
    private readonly IStringListStemmer _stringListStemmer =
        stringListStemmer ?? throw new ArgumentNullException(nameof(stringListStemmer));
    
    private readonly IStringTrimAndSplitter _stringTrimAndSplitter =
        stringTrimAndSplitter ?? throw new ArgumentNullException(nameof(stringTrimAndSplitter));
    
    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        var result = _stringTrimAndSplitter.TrimAndSplit(source);
        result = _stringListNoiseCleaner.CleanNoise(result);
        result = _stringListCleaner.Clean(result);
        result = _stringListStemmer.Stem(result);
        result = _stringListNonValidWordCleaner.Clean(result);
        return result.Distinct();
    }
}
