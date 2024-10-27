using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.RegexCheckers;
using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.StringCleaners.NoiseCleaner.Abstractions;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex.Services;

internal sealed class StringToWordsProcessor(IPorter2Stemmer stemmer,INoiseCleaner noiseCleaner) : IStringToWordsProcessor
{
    private readonly List<string>    _banned       = AppSettings.BannedWords.ToList();
    private readonly IPorter2Stemmer _stemmer      = stemmer ?? throw new ArgumentNullException(nameof(stemmer));
    private readonly INoiseCleaner _noiseCleaner = noiseCleaner ?? throw new ArgumentNullException(nameof(noiseCleaner));

    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        var result = CleanAndSplit(source);
        result = _noiseCleaner.CleanNoise(result);
        result = CleanAndSelect(result);
        result = Stem(result);
        result = PurgeNonValidWords(result);
        return result.Distinct();
    }

    private static IEnumerable<string> CleanAndSplit(string source) => source.Trim().Split();
    
    private static IEnumerable<string> CleanAndSelect(IEnumerable<string> value)
    {
        return value.Select(Cleanse).SelectMany(x => x); // flatten the string arrays
            
    }
    private IEnumerable<string> Stem(IEnumerable<string> value) => value.Select(Stem);
    private IEnumerable<string> PurgeNonValidWords(IEnumerable<string> value) => value.Where(IsValid);
    

    public void Construct(IEnumerable<string>? banned)
    {
        if (banned != null)
            _banned.AddRange(banned);
    }

    private bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && IsLongEnough(value) && IsNotBanned(value);

    private bool IsNotBanned(string value) => !_banned.Contains(value);

    private static bool IsLongEnough(string value) => value.Length >= 3;

    private string Stem(string value) => _stemmer.Stem(value).Value.ToLower();

    private static string[] Cleanse(string value)
    {
        value = TrimSpecialCharacters(value);

        if (!IsNumberAndMoreThan2Digits(value))
            value = TrimDigits(value);

        return SplitStringViaSpecialCharacters(value);
    }


    private static string[] SplitStringViaSpecialCharacters(string value) =>
        value.Split(AppSettings.SplitterSpecialCharacters);

    private static string TrimDigits(string value) => value.Trim("012345689".ToCharArray());

    private static string TrimSpecialCharacters(string value) => value.Trim(AppSettings.TrimableSpecialCharacters);

    private static bool IsNumberAndMoreThan2Digits(string value) =>
        double.TryParse(value, out var _) && value.Length >= 3;
}
