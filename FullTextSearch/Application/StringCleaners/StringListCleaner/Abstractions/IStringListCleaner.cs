namespace FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;

internal interface IStringListCleaner
{
    IEnumerable<string> Clean(IEnumerable<string> value);
}
