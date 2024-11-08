using FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;
using Porter2Stemmer;

namespace FullTextSearch.Application.StringCleaners.StringListStemmer.Services;

internal class StringListStemmer(IPorter2Stemmer stemmer) : IStringListStemmer
{
    private readonly IPorter2Stemmer _stemmer = stemmer ?? throw new ArgumentNullException(nameof(stemmer));
    
    public IEnumerable<string> Stem(IEnumerable<string> value) => value.Select(Stem);
    private string Stem(string value) => _stemmer.Stem(value).Value.ToLower();
}
