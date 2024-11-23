namespace FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;

internal interface IStringListNonValidWordCleaner
{
    IEnumerable<string> Clean(IEnumerable<string> value);
}
