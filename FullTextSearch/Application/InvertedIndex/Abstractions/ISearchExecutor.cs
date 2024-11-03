using FullTextSearch.Application.Models;

namespace FullTextSearch.Application.InvertedIndex.Abstractions;

internal interface ISearchExecutor
{
    void Construct(Dictionary<string, List<string>> invertedIndex);
    IEnumerable<string> ExecuteSearch(ProcessedQueryWords processedWords);
}
