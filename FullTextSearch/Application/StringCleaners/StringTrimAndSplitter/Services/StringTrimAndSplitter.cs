using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;

namespace FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Services;

internal class StringTrimAndSplitter : IStringTrimAndSplitter
{
    public IEnumerable<string> TrimAndSplit(string source) => source.Trim().Split();
}
