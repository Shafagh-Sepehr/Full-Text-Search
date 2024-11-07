using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Services;

namespace FullTextSearch.Tests.Application.DocumentsReader;

public class AndDocumentsReaderTests
{
    private readonly AndDocumentsReader _reader = new AndDocumentsReader();

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
    public void GetAndDocuments_WhenCorrectlyCalled_ShouldReturnIntersectedDocuments(IReadOnlyList<string> andWords, HashSet<string> expectedResult)
    {
        //Act
        var result = _reader!.GetAndDocuments(_invertedIndex, andWords);
        
        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return [new[] { "green", "blue", }, new HashSet<string> { "1", "2", },];
        yield return [new[] { "red", "blue", }, new HashSet<string> { "1", "2", },];
        yield return [new[] { "green", "orange", }, new HashSet<string> { "2", },];
        yield return [new[] { "green", "black", }, new HashSet<string>(),];
        yield return [new[] { "green", "yellow", "brown", }, new HashSet<string>(),];
        yield return [new[] { "green", "orange", "purple", }, new HashSet<string> { "2", },];
        yield return [new[] { "green", "orange", "purple", "brown", }, new HashSet<string>(),];
        yield return [new[] { "green", "yellow", }, new HashSet<string>(),];
        yield return [new[] { "green", }, new HashSet<string>{"1","2","3"},];
        yield return [Array.Empty<string>(), new HashSet<string>(),];
    }
}
