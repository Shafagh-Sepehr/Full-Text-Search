namespace FullTextSearch;

internal interface IAppSettings
{
    string                documentsPath { get; }
    IReadOnlyList<string>? bannedWords   { get; }
}
