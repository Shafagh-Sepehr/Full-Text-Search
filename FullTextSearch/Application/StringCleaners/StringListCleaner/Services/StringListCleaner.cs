using FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;

namespace FullTextSearch.Application.StringCleaners.StringListCleaner.Services;

internal class StringListCleaner : IStringListCleaner
{
    public IEnumerable<string> Clean(IEnumerable<string> value) =>
        value.Select(Cleanse).SelectMany(x => x); // flatten the string arrays

    private static string[] Cleanse(string value)
    {
        value = TrimSpecialCharacters(value);

        if (!IsNumberAndMoreThan2Digits(value))
            value = TrimDigits(value);

        return SplitStringViaSpecialCharacters(value);
    }

    private static string[] SplitStringViaSpecialCharacters(string value) =>
        value.Split(AppSettings.SplitterSpecialCharacters);

    private static string TrimDigits(string value) => value.Trim("0123456789".ToCharArray());

    private static string TrimSpecialCharacters(string value) => value.Trim(AppSettings.TrimableSpecialCharacters);

    private static bool IsNumberAndMoreThan2Digits(string value) =>
        double.TryParse(value, out var _) && value.Length >= 3;
}
