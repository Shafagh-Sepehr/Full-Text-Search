using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FullTextSearch.Tests.Application.DocumentsReader;

public class OrDocumentsReaderTests
{
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
    public void GetOrDocuments_returnsUnionOfDocuments(IReadOnlyList<string> andWords, HashSet<string> expectedResult)
    {
        //Arrange
        var reader = ServiceCollection.ServiceProvider.GetService<IOrDocumentsReader>();

        //pre Assert
        reader.Should().NotBeNull();

        //Act
        var result = reader!.GetOrDocuments(_invertedIndex, andWords);

        //Assert
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
