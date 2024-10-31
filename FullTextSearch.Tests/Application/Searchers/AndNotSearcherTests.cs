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

    private readonly Dictionary<string, List<string>> _originalInvertedIndex;
    private readonly QueryProcessedWords              _originalWords;

    public AndNotSearcherTests()
    {
        _documentReader = Substitute.For<IDocumentReader>();
        _andOrNotSearcher = new(_documentReader);

        _originalInvertedIndex = new()
        {
            { "word1", ["doc1", "doc2",] },
            { "word2", ["doc3",] },
        };
        _originalWords = new()
        {
            AndWords = ["andword1", "andword2",],
            OrWords = [],
            NotWords = ["notword1", "notword2",],
        };
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void AndOrNotSearch_ReturnsAndDocsIntersectOrDocsExceptNotDocs_ShouldNotModifyInputValues(
        HashSet<string> andDocs, HashSet<string> notDocs, IReadOnlySet<string> expectedResult)
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var words = new QueryProcessedWords
        {
            AndWords = _originalWords.AndWords,
            OrWords = _originalWords.OrWords,
            NotWords = _originalWords.NotWords,
        };

        _documentReader.GetAndDocuments(invertedIndex, words.AndWords).Returns(andDocs);
        _documentReader.GetNotDocuments(invertedIndex, words.NotWords).Returns(notDocs);

        // Act
        var result = _andOrNotSearcher.AndNotSearch(invertedIndex, words);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _documentReader.Received(1).GetAndDocuments(invertedIndex, words.AndWords);
        _documentReader.Received(0).GetOrDocuments(invertedIndex, words.OrWords);
        _documentReader.Received(1).GetNotDocuments(invertedIndex, words.NotWords);

        // Verify that the original inputs are unchanged
        words.Should().BeEquivalentTo(_originalWords);
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
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
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IDocumentReader documentReader = null!;

        // Act & Assert
        Action act = () => new AndNotSearcher(documentReader);
        act.Should().Throw<ArgumentNullException>();
    }
}
