namespace FullTextSearch.Application.StringCleaners.WordListStemmer.Abstractions;

internal interface IWordListStemmer
{
    IEnumerable<string> Stem(IEnumerable<string> value);
}
