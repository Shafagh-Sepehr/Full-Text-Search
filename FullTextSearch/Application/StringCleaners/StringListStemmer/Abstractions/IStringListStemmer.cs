namespace FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;

internal interface IStringListStemmer
{
    IEnumerable<string> Stem(IEnumerable<string> value);
}
