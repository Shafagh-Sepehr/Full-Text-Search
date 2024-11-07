using FluentAssertions;
using FullTextSearch.Application.WordsProcessors.Services;
using NSubstitute;
using Porter2Stemmer;

namespace FullTextSearch.Tests.Application.WordProcessors;

public class PrefixBasedNotWordsProcessorTests
{
    private readonly PrefixBasedNotWordsProcessor _wordsProcessor;

    public PrefixBasedNotWordsProcessorTests()
    {
        //Arrange

        var stemmer = Substitute.For<IPorter2Stemmer>();

        //returns input without any change
        stemmer.Stem(Arg.Any<string>()).Returns(callInfo =>
        {
            var input = callInfo.Arg<string>();
            return new(input, input);
        });

        _wordsProcessor = new(stemmer);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void GetNotDocuments_WhenCorrectlyCalled_ShouldReturnsIntersectedDocuments(string[] queryWords, IReadOnlyList<string> expectedResult)
    {
        //Act
        var result = _wordsProcessor.GetNotWords(queryWords);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }


    public static IEnumerable<object?[]> TestData()
    {
        yield return [new[] { "-word1", "-word2", }, new List<string> { "word1", "word2", },];
        yield return [new[] { "word1", "-word2", }, new List<string> { "word2", },];
        yield return [new[] { "word1", "+word2", }, new List<string> {  },];
        yield return [new[] { "-word1", "+word2", }, new List<string>{"word1"},];
        yield return [new[] { "word1", "word2", "-word3", "-word4", "+word5", }, new List<string>{"word3", "word4", },];
        yield return [Array.Empty<string>(), new List<string>(),];
    }
    
    [Fact]
    public void Constructor_WhenDependenciesAreNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IPorter2Stemmer stemmer = null!;

        // Act & Assert
        Action act = () => new PrefixBasedNotWordsProcessor(stemmer);
        act.Should().Throw<ArgumentNullException>();
    }
}
