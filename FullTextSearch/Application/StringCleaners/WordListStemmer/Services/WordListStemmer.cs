using FullTextSearch.Application.StringCleaners.WordListStemmer.Abstractions;
using Porter2Stemmer;

namespace FullTextSearch.Application.StringCleaners.WordListStemmer.Services;

internal class WordListStemmer(IPorter2Stemmer stemmer) : IWordListStemmer
{
    private readonly IPorter2Stemmer _stemmer = stemmer ?? throw new ArgumentNullException(nameof(stemmer));

    public IEnumerable<string> Stem(IEnumerable<string> value) => value.Select(Stem);
    private string Stem(string value) => _stemmer.Stem(value).Value.ToLower();
}
