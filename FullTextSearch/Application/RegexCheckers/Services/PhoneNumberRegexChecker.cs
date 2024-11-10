using FullTextSearch.Application.RegexCheckers.Abstractions;

namespace FullTextSearch.Application.RegexCheckers.Services;

internal class PhoneNumberRegexChecker : IPhoneNumberRegexChecker
{
    public bool Matches(string value) =>
        AppSettings.RegexPatterns.PhoneNumberRegex().IsMatch(value);
}
