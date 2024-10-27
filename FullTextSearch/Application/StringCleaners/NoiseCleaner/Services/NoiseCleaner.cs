using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.StringCleaners.NoiseCleaner.Abstractions;

namespace FullTextSearch.Application.StringCleaners.NoiseCleaner.Services;

internal class NoiseCleaner(IRegexChecker regexChecker) : INoiseCleaner
{
    private readonly IRegexChecker _regexChecker = regexChecker ?? throw new ArgumentNullException(nameof(regexChecker));
    
    public IEnumerable<string> CleanNoise(IEnumerable<string> value) => value.Where(IsNotNoise);
    
    
    private bool IsNotNoise(string value) =>
        !_regexChecker.HasEmail(value) &&
        !_regexChecker.HasUrl(value) &&
        !_regexChecker.HasPhoneNumber(value);
}
