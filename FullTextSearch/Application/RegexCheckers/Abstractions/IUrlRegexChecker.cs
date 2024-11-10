namespace FullTextSearch.Application.RegexCheckers.Abstractions;

internal interface IUrlRegexChecker
{
    bool Matches(string value);
}
