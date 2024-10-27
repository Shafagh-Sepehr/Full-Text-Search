using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class StringToWordsProcessor(IStringListNoiseCleaner stringListNoiseCleaner, IStringTrimAndSplitter stringTrimAndSplitter, IStringListCleaner stringListCleaner,IStringListStemmer stringListStemmer)
    : IStringToWordsProcessor
{
    private readonly List<string>     _banned          = AppSettings.BannedWords.ToList();
    private readonly IStringListNoiseCleaner    _stringListNoiseCleaner    = stringListNoiseCleaner ?? throw new ArgumentNullException(nameof(stringListNoiseCleaner));
    private readonly IStringListCleaner   _stringListCleaner   = stringListCleaner ?? throw new ArgumentNullException(nameof(stringListCleaner));
    private readonly IStringListStemmer _stringListStemmer = stringListStemmer ?? throw new ArgumentNullException(nameof(stringListStemmer));

    private readonly IStringTrimAndSplitter _stringTrimAndSplitter =
        stringTrimAndSplitter ?? throw new ArgumentNullException(nameof(stringTrimAndSplitter));

    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        var result = _stringTrimAndSplitter.TrimAndSplit(source);
        result = _stringListNoiseCleaner.CleanNoise(result);
        result = _stringListCleaner.Clean(result);
        result = _stringListStemmer.Stem(result);
        result = PurgeNonValidWords(result);
        return result.Distinct();
    }


    public void Construct(IEnumerable<string>? banned)
    {
        if (banned != null)
            _banned.AddRange(banned);
    }

    private IEnumerable<string> PurgeNonValidWords(IEnumerable<string> value) => value.Where(IsValid);

    private bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && IsLongEnough(value) && IsNotBanned(value);

    private bool IsNotBanned(string value) => !_banned.Contains(value);

    private static bool IsLongEnough(string value) => value.Length >= 3;
}
