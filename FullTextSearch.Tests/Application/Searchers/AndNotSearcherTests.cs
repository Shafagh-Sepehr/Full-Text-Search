using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.Searchers;

public class AndNotSearcherTests
{
    private readonly AndNotSearcher  _andOrNotSearcher;
    private readonly IDocumentReader _documentReader;

    public AndNotSearcherTests()
    {
        _documentReader = Substitute.For<IDocumentReader>();
        _andOrNotSearcher = new(_documentReader);

    }


    [Theory]
    [MemberData(nameof(TestData))]
    public void AndNotSearch_WhenCorrectlyCalled_ShouldCallMethodsInOrderAndShouldReturnCorrectDocs(
        HashSet<string> andDocs, HashSet<string> notDocs, IReadOnlySet<string> expectedResult)
    {
        // Arrange
        IReadOnlyDictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();
        var words = new ProcessedQueryWords
        {
            AndWords = ["andword1", "andword2",],
            OrWords = [],
            NotWords = ["notword1", "notword2",],
        };

        _documentReader.GetAndDocuments(invertedIndex, words.AndWords).Returns(andDocs);
        _documentReader.GetNotDocuments(invertedIndex, words.NotWords).Returns(notDocs);

        // Act
        var result = _andOrNotSearcher.AndNotSearch(invertedIndex, words);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        Received.InOrder(() =>
        {
            _documentReader.GetAndDocuments(invertedIndex, words.AndWords);
            _documentReader.GetNotDocuments(invertedIndex, words.NotWords);
        });
        _documentReader.DidNotReceiveWithAnyArgs().GetOrDocuments(default!, default!);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return [new HashSet<string> { "1", "2", }, new HashSet<string>(), new HashSet<string> { "1", "2", },];
        yield return [new HashSet<string> { "1", "2", }, new HashSet<string> { "1", }, new HashSet<string> { "2", },];
        yield return [new HashSet<string> { "1", "2", "3", }, new HashSet<string> { "1", "2", }, new HashSet<string> { "3", },];
        yield return [new HashSet<string> { "1", "2", "3", "4", }, new HashSet<string> { "1", "2", }, new HashSet<string> { "3", "4", },];
        yield return [new HashSet<string> { "1", "2", }, new HashSet<string> { "3", "4", }, new HashSet<string> { "2", "1", },];
        yield return [new HashSet<string> { "1", "2", }, new HashSet<string> { "1", "2", }, new HashSet<string>(),];
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IDocumentReader documentReader = null!;

        // Act
        Action act = () => new AndNotSearcher(documentReader);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
