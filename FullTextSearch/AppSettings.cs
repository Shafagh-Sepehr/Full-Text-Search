using System.Text.RegularExpressions;

namespace FullTextSearch;

internal static partial class AppSettings
{
    public static readonly string[] BannedWords =
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
    
    public static readonly char[] TrimableSpecialCharacters = ":.,<>?/\\\"\t'`~!@#$%^&*()_+=-*;\t |".ToArray();
    
    public static readonly char[] SplitterSpecialCharacters = "-_'()[]'\";:/,\\><=".ToArray();
    
    internal static partial class RegexPatterns
    {
        [GeneratedRegex(
            @"(?<!\S)(?:(?:https?|ftp):\/\/|www\.|(?:\d{1,3}\.){3}\d{1,3}(?::\d{1,5})?\/|localhost(?::\d{1,5})?|[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?::\d{1,5})?)(?:[^\s()<>]+(?:$[\w\d]+$|([^[:punct:]\s]|\/))*)(?=\s|$)")]
        public static partial Regex UrlRegex();
        
        [GeneratedRegex(
            "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])")]
        public static partial Regex EmailRegex();
        
        [GeneratedRegex(
            @"^(?:\+?\d{1,3}[-.\s]?)?(?:$\d{1,4}$[-\s]?|\d{1,4}[-.\s]?)?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}(?:\s?(?:ext|x|extension|#)?\.?\s?\d{1,5})?;?$")]
        public static partial Regex PhoneNumberRegex();
    }
}
