using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.StringCleaners.NoiseCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;
using FullTextSearch.Application.StringCleaners.WordListStemmer;
using FullTextSearch.Application.StringCleaners.WordListStemmer.Abstractions;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class StringToWordsProcessor(INoiseCleaner noiseCleaner, IStringTrimAndSplitter stringTrimAndSplitter, IStringCleaner stringCleaner,IWordListStemmer wordListStemmer)
    : IStringToWordsProcessor
{
    private readonly List<string>     _banned          = AppSettings.BannedWords.ToList();
    private readonly INoiseCleaner    _noiseCleaner    = noiseCleaner ?? throw new ArgumentNullException(nameof(noiseCleaner));
    private readonly IStringCleaner   _stringCleaner   = stringCleaner ?? throw new ArgumentNullException(nameof(stringCleaner));
    private readonly IWordListStemmer _wordListStemmer = wordListStemmer ?? throw new ArgumentNullException(nameof(wordListStemmer));

    private readonly IStringTrimAndSplitter _stringTrimAndSplitter =
        stringTrimAndSplitter ?? throw new ArgumentNullException(nameof(stringTrimAndSplitter));

    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        var result = _stringTrimAndSplitter.TrimAndSplit(source);
        result = _noiseCleaner.CleanNoise(result);
        result = _stringCleaner.Clean(result);
        result = _wordListStemmer.Stem(result);
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
