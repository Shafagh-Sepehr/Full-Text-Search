namespace FullTextSearch.Application.Searchers.DataViewModels;

internal class Words
{
    public required List<string> AndWords { get; init; }
    public required List<string> OrWords  { get; init; }
    public required List<string> NotWords { get; init; }
    
}
