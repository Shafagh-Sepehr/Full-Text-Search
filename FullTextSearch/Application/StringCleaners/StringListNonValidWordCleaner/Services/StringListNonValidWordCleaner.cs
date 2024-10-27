using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;

namespace FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Services;

internal class StringListNonValidWordCleaner : IStringListNonValidWordCleaner
{
    private readonly List<string> _banned = AppSettings.BannedWords.ToList();

    public void Construct(IEnumerable<string>? banned)
    {
        if (banned != null)
            _banned.AddRange(banned);
    }

    public IEnumerable<string> Clean(IEnumerable<string> value) => value.Where(IsValid);
    private bool IsValid(string value) => !string.IsNullOrWhiteSpace(value) && IsLongEnough(value) && IsNotBanned(value);
    private bool IsNotBanned(string value) => !_banned.Contains(value);
    private static bool IsLongEnough(string value) => value.Length >= 3;
}
