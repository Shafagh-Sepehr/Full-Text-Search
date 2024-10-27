namespace FullTextSearch.Application.RegexCheckers.Abstractions;

internal interface IRegexChecker
{
    bool HasEmail(string value);
    bool HasPhoneNumber(string value);
    bool HasUrl(string value);
}
