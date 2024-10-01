using CodeStar2;
using FluentAssertions;
using NSubstitute;
using Porter2Stemmer;

namespace FullTextSearch.Tests;

public class QuerySearcherTests
{
    private readonly Dictionary<string, List<string>> _invertedIndex = new()
    {
        { "blue", ["1", "2", "3", "4", "5", "6",] },
        { "purple", ["3", "4", "5", "6",] },
        { "red", ["1", "7",] },
        { "green", ["1", "2",] },
        { "yellow", ["1", "8",] },
        { "black", ["3", "4",] },
    };

    private readonly IPorter2Stemmer _stemmer = Substitute.For<IPorter2Stemmer>();


    [Theory]
    [MemberData(nameof(TestData))]
    public void TestDifferentQueries(string query, string[] expectedResult)
    {
        //Arrange
        _stemmer.Stem(Arg.Any<string>()).Returns(callInfo =>
        {
            var input = callInfo.Arg<string>();
            return new StemmedWord(input, input);
        });
        var querySearcher = new QuerySearcher(_stemmer);


        //Act
        IEnumerable<string> documents = querySearcher.Search(query, _invertedIndex);

        //Assert
        documents.Should().BeEquivalentTo(expectedResult);
    }


    public static IEnumerable<object[]> TestData()
    {
        yield return ["blue +red +green -yellow", new[] { "2", },];
        yield return ["+red", new[] { "1", "7", },];
        yield return ["+blue +red -purple", new[] { "2", "1", "7", },];
        yield return ["green yellow", new[] { "1", },];
        yield return ["green", new[] { "1", "2", },];
        yield return ["blue purple -yellow", new[] { "3", "4", "5", "6", },];
        yield return ["blue purple +black -yellow", new[] { "3", "4", },];
        yield return ["blue purple -black", new[] { "5", "6", },];
        yield return ["blue purple", new[] { "3", "4", "5", "6", },];
        yield return ["-green", Array.Empty<object>(),];
        yield return ["blue purple +green +red +yellow", Array.Empty<object>(),];
        yield return ["blue purple +yellow", Array.Empty<object>(),];
        yield return ["green yellow -blue", Array.Empty<object>(),];
        yield return ["", Array.Empty<object>(),];
        yield return ["non-existent-word-returns-empty-list", Array.Empty<object>(),];
    }
}
