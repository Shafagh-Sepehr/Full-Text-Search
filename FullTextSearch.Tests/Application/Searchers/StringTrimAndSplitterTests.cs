using FluentAssertions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Services;

namespace FullTextSearch.Tests.Application.Searchers;

public class StringTrimAndSplitterTests
{
    private readonly StringTrimAndSplitter _stringTrimAndSplitter = new();

    [Theory]
    [MemberData(nameof(TestData))]
    public void TrimAndSplit_WhenCorrectlyCalled_ShouldReturnListOfTrimmedSplitedString(string input, IEnumerable<string> expectedResult)
    {
        //Act
        var result = _stringTrimAndSplitter.TrimAndSplit(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return ["", new List<string> { "", },];
        yield return ["hello", new List<string> { "hello", },];
        yield return ["hello      ", new List<string> { "hello", },];
        yield return ["    hello", new List<string> { "hello", },];
        yield return ["    hello      ", new List<string> { "hello", },];
        yield return ["hello world", new List<string> { "hello", "world", },];
        yield return ["hello  world", new List<string> { "hello", "", "world", },];
        yield return ["    hello world      ", new List<string> { "hello", "world", },];
        yield return ["    hello, world!      ", new List<string> { "hello,", "world!", },];
    }
}
