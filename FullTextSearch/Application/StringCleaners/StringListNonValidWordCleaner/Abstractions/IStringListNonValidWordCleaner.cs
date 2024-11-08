namespace FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;

internal interface IStringListNonValidWordCleaner
{
    void Construct(IReadOnlyList<string>? bannedWords);
    IEnumerable<string> Clean(IEnumerable<string> value);
}
