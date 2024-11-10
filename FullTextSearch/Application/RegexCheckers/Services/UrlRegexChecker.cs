using FullTextSearch.Application.RegexCheckers.Abstractions;

namespace FullTextSearch.Application.RegexCheckers.Services;

internal class UrlRegexChecker : IUrlRegexChecker
{
    public bool Matches(string value) =>
        AppSettings.RegexPatterns.UrlRegex().IsMatch(value);
}
