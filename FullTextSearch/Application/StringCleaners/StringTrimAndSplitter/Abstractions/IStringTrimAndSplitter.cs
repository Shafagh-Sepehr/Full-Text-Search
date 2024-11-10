namespace FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;

internal interface IStringTrimAndSplitter
{
    IEnumerable<string> TrimAndSplit(string source);
}
