namespace FullTextSearch.Application.StringCleaners.StringCleaner.Abstractions;

internal interface IStringCleaner
{
    IEnumerable<string> Clean(IEnumerable<string> value);
}
