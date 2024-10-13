namespace FullTextSearch.Application.Searchers.DataViewModels;

internal sealed class Words
{
    public required IReadOnlyList<string> AndWords { get; init; }
    public required IReadOnlyList<string> OrWords  { get; init; }
    public required IReadOnlyList<string> NotWords { get; init; }
    
}
