using FullTextSearch.Application.RegexCheckers.Abstractions;

namespace FullTextSearch.Application.RegexCheckers.Services;

internal class EmailRegexChecker : IEmailRegexChecker
{
    public bool Matches(string value) =>
        AppSettings.RegexPatterns.EmailRegex().IsMatch(value);
}
