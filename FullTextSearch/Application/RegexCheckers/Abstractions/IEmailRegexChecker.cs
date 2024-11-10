namespace FullTextSearch.Application.RegexCheckers.Abstractions;

internal interface IEmailRegexChecker
{
    bool Matches(string value);
}
