namespace FullTextSearch.Application.Models;

internal sealed record ProcessedQueryWords
{
    public required IReadOnlyList<string> AndWords { get; init; }
    public required IReadOnlyList<string> OrWords  { get; init; }
    public required IReadOnlyList<string> NotWords { get; init; }
}
