namespace FullTextSearch.Application.StringCleaners.NoiseCleaner.Abstractions;

internal interface INoiseCleaner
{
    IEnumerable<string> CleanNoise(IEnumerable<string> value);
}
