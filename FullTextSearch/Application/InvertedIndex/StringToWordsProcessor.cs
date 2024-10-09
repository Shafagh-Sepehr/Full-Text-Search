using System.Text.RegularExpressions;
using FullTextSearch.Application.InvertedIndex.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch.Application.InvertedIndex;

internal partial class StringToWordsProcessor : IStringToWordsProcessor
{
    private readonly IPorter2Stemmer _stemmer;
    private readonly List<string>    _banned = AppSettings.BannedWords.ToList();
    

    
    
    public StringToWordsProcessor(IEnumerable<string>? banned, IPorter2Stemmer stemmer)
    {
        _stemmer = stemmer;
        if (banned != null) 
            _banned.AddRange(banned);
    }
    
    
    
    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        
        return source
            .Trim().Split().Where(IsNotNoise)
            .Select(Cleanse).SelectMany(x => x) // flatten the string arrays
            .Select(Stem).Where(IsValid).Distinct();
    }

    private bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && IsLongEnough(value) && IsNotBanned(value);
    
    private bool IsNotBanned(string value) => !_banned.Contains(value);

    private static bool IsLongEnough(string value) => value.Length >= 3;


    private static bool IsNotNoise(string value) =>
        !AppSettings.RegexPatterns.UrlRegex().IsMatch(value) &&
        !AppSettings.RegexPatterns.EmailRegex().IsMatch(value) &&
        !AppSettings.RegexPatterns.PhoneNumberRegex().IsMatch(value);

    private string Stem(string value) => _stemmer.Stem(value).Value.ToLower();
    
    private static string[] Cleanse(string value)
    {
        value = TrimSpecialCharacters(value);

        if (!IsNumberAndMoreThan2Digits(value))
            value = TrimDigits(value);
        
        return SplitStringViaSpecialCharacters(value);
    }


    private static string[] SplitStringViaSpecialCharacters(string value) => value.Split(AppSettings.SplitterSpecialCharacters);

    private static string TrimDigits(string value) => value.Trim("012345689".ToCharArray());

    private static string TrimSpecialCharacters(string value) => value.Trim(AppSettings.TrimableSpecialCharacters);
    
    private static bool IsNumberAndMoreThan2Digits(string value) =>
        double.TryParse(value, out double result) && value.Length >= 3;
}
