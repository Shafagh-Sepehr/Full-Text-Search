namespace FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;

internal interface IStringListNonValidWordCleaner
{
    void Construct(IEnumerable<string>? banned);
    IEnumerable<string> Clean(IEnumerable<string> value);
}
