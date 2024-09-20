using System.Text.RegularExpressions;
using Porter2Stemmer;

namespace CodeStar2;

public partial class StringToWordsProcessor
{
    private readonly List<string> _banned =
    [
        "i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "your", "yours", "yourself",
        "yourselves", "he", "him", "his", "himself", "she", "her", "hers", "herself", "it", "its", "itself", "they",
        "them", "their", "theirs", "themselves", "what", "which", "who", "whom", "this", "that", "these", "those",
        "am", "is", "are", "was", "were", "be", "been", "being", "have", "has", "had", "having", "do", "does",
        "did", "doing", "a", "an", "the", "and", "but", "if", "or", "because", "as", "until", "while", "of", "at",
        "by", "for", "with", "about", "against", "between", "into", "through", "during", "before", "after", "above",
        "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", "under", "again", "further", "then",
        "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each", "few", "more", "most",
        "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than", "too", "very", "s", "t",
        "can", "will", "just", "don", "should", "now",
    ];
    
    [GeneratedRegex(@"(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?\/[a-zA-Z0-9]{2,}|((https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?)|(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,})?")]
    private partial Regex UrlRegex();
    [GeneratedRegex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])")]
    private partial Regex EmailRegex();
    [GeneratedRegex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")]
    private partial Regex PhoneNumberRegex();

    
    
    public StringToWordsProcessor(List<string>? banned)
    {
        if (banned != null) 
            _banned.AddRange(banned);
    }
    
    
    
    public IEnumerable<string> TrimSplitAndStemString(string source, IPorter2Stemmer stemmer)
    {
        
        return source
            .Trim()
            .Split()
            .Where(x => !UrlRegex().IsMatch(x) || !EmailRegex().IsMatch(x) || !PhoneNumberRegex().IsMatch(x))
            .Select(CleanseString).SelectMany(x => x) // flatten the string arrays
            .Select(x => stemmer.Stem(x).Value.ToLower())
            .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 3 && !_banned.Contains(x))
            .Distinct();
    }
    
    private string[] CleanseString(string value)
    {
        value = value.Trim(":.,<>?/\\\"\t'`~!@#$%^&*()_+=-*;\t |".ToCharArray()); //trim noise around probably important words
        
        if (!IsNumberAndMoreThan2Digits(value))
            value = value.Trim("012345689".ToCharArray()); //trim digits of a string if the string isn't at least a two-digit number
        
        return value.Split("-_.'()[]'\";:./,\\><".ToCharArray()); //split the string to extract words seperated by charachters other than space
    }
    
    private bool IsNumberAndMoreThan2Digits(string value) => double.TryParse(value, out var result) && value.Length >= 3;
}
