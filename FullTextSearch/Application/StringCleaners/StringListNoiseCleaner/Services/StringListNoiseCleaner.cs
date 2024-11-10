using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;

namespace FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Services;

internal class StringListNoiseCleaner(IRegexChecker regexChecker) : IStringListNoiseCleaner
{
    private readonly IRegexChecker _regexChecker = regexChecker ?? throw new ArgumentNullException(nameof(regexChecker));
    
    public IEnumerable<string> CleanNoise(IEnumerable<string> value) => value.Where(IsNotNoise);
    
    private bool IsNotNoise(string value) =>
        !_regexChecker.HasEmail(value) &&
        !_regexChecker.HasUrl(value) &&
        !_regexChecker.HasPhoneNumber(value);
}
