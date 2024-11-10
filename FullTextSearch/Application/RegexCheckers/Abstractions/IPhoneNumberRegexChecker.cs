namespace FullTextSearch.Application.RegexCheckers.Abstractions;

internal interface IPhoneNumberRegexChecker
{
    bool Matches(string value);
}
