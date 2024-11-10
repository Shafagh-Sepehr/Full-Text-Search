using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface ISearchExecutor
{
    void Construct(IReadOnlyDictionary<string, List<string>> invertedIndex);
    IReadOnlySet<string> ExecuteSearch(ProcessedQueryWords processedWords);
}
