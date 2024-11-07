using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Services;

namespace FullTextSearch.Tests.Application.DocumentsReader;

public class NotDocumentsReaderTests
{
    private readonly NotDocumentsReader _reader = new NotDocumentsReader();

    private readonly Dictionary<string, List<string>> _invertedIndex = new()
    {
        { "green", ["1", "2", "3",] },
        { "blue", ["1", "2",] },
        { "red", ["1", "2",] },
        { "orange", ["2", "4", "5",] },
        { "yellow", ["4", "5", "6",] },
        { "brown", ["7", "8", "9",] },
        { "purple", ["2", "10", "11",] },
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void GetNotDocuments_WhenCorrectlyCalled_ShouldReturnUnionOfDocuments(IReadOnlyList<string> notWords, HashSet<string> expectedResult)
    {
        // Act
        var result = _reader!.GetNotDocuments(_invertedIndex, notWords);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return [new[] { "green", "blue", }, new HashSet<string> { "1", "2", "3", },];
        yield return [new[] { "red", "blue", }, new HashSet<string> { "1", "2", },];
        yield return [new[] { "green", "orange", }, new HashSet<string> { "1", "2", "3", "4", "5", },];
        yield return [new[] { "green", "black", }, new HashSet<string> { "1", "2", "3", },];
        yield return [new[] { "green", "yellow", "brown", }, new HashSet<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", },];
        yield return [new[] { "green", "orange", "purple", }, new HashSet<string> { "1", "2", "3", "4", "5", "10", "11", },];
        yield return [new[] { "green", "orange", "purple", "brown", }, new HashSet<string> { "1", "2", "3", "4", "5", "7", "8", "9", "10", "11", },];
    }
}
