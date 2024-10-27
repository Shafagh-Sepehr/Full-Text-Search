namespace FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;

internal interface IStringListNoiseCleaner
{
    IEnumerable<string> CleanNoise(IEnumerable<string> value);
}
