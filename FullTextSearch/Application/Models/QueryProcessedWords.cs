namespace FullTextSearch.Application.Models;

internal sealed class QueryProcessedWords
{
    public required IReadOnlyList<string> AndWords { get; init; }
    public required IReadOnlyList<string> OrWords  { get; init; }
    public required IReadOnlyList<string> NotWords { get; init; }
    
}
