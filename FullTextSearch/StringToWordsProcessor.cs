using System.Text.RegularExpressions;
using FullTextSearch.Interfaces;
using Porter2Stemmer;

namespace FullTextSearch;

internal partial class StringToWordsProcessor : IStringToWordsProcessor
{
    private readonly IPorter2Stemmer _stemmer;
    private readonly List<string>    _banned = AppSettings.BannedWords.ToList();
    
    [GeneratedRegex(@"(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?\/[a-zA-Z0-9]{2,}|((https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?)|(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,})?")]
    private partial Regex UrlRegex();
    [GeneratedRegex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])")]
    private partial Regex EmailRegex();
    [GeneratedRegex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")]
    private partial Regex PhoneNumberRegex();

    
    
    public StringToWordsProcessor(IEnumerable<string>? banned, IPorter2Stemmer stemmer)
    {
        _stemmer = stemmer;
        if (banned != null) 
            _banned.AddRange(banned);
    }
    
    
    
    public IEnumerable<string> TrimSplitAndStemString(string source)
    {
        
        return source
            .Trim()
            .Split()
            .Where(IsNoise)
            .Select(CleanseString).SelectMany(x => x) // flatten the string arrays
            .Select(x => Stem(x))
            .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 3 && !_banned.Contains(x))
            .Distinct();
    }
    
    private bool IsNoise(string value) =>
        !UrlRegex().IsMatch(value) && !EmailRegex().IsMatch(value) && !PhoneNumberRegex().IsMatch(value);

    private string Stem(string value) => _stemmer.Stem(value).Value.ToLower();
    
    private string[] CleanseString(string value)
    {
        value = value.Trim(":.,<>?/\\\"\t'`~!@#$%^&*()_+=-*;\t |".ToCharArray()); //trim noise around probably important words
        
        if (!IsNumberAndMoreThan2Digits(value))
            value = value.Trim("012345689".ToCharArray()); //trim digits of a string if the string isn't at least a two-digit number
        
        return value.Split("-_'()[]'\";:/,\\><=".ToCharArray()); //split the string to extract words seperated by characters other than space
    }
    
    private static bool IsNumberAndMoreThan2Digits(string value) =>
        double.TryParse(value, out double result) && value.Length >= 3;
}
