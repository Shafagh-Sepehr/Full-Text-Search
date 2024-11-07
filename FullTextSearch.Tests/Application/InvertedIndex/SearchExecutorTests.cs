using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Exceptions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class SearchExecutorTests
{
    private readonly ISearcher      _searcher;
    private readonly SearchExecutor _searchExecutor;

    private readonly Dictionary<string, List<string>> _originalInvertedIndex  = new() { { "key", ["val1", "val2",] }, };
    private readonly HashSet<string>                  _originalExpectedResult = ["words",];

    public SearchExecutorTests()
    {
        _searcher = Substitute.For<ISearcher>();
        _searchExecutor = new(_searcher);
    }

    [Fact]
    public void ExecuteSearch_WhenConstructMethodIsNotCalled_ShouldThrowException()
    {
        //Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = [],
            NotWords = [],
        };

        //Act
        Action act = () => _searchExecutor.ExecuteSearch(words);

        //Assert
        act.Should().Throw<ConstructMethodNotCalledException>();
    }

    [Fact]
    public void ExecuteSearch_WhenAllWordsPresent_ShouldCallAndOrNotSearchAndShouldNotModifyInputAndReturnValues()
    {
        //Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = ["word",],
            OrWords = ["word",],
            NotWords = ["word",],
        };
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var expected = new HashSet<string>(_originalExpectedResult);
        _searchExecutor.Construct(invertedIndex);
        _searcher.AndOrNotSearch(invertedIndex, words).Returns(expected);

        //Act
        var result = _searchExecutor.ExecuteSearch(words);

        //Assert
        result.Should().BeEquivalentTo(expected);
        _searcher.Received(1).AndOrNotSearch(invertedIndex, words);

        expected.Should().BeEquivalentTo(_originalExpectedResult);
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
    }

    [Fact]
    public void ExecuteSearch_WhenAndWordsPresent_ShouldCallAndNotSearchAndShouldNotModifyInputAndReturnValues()
    {
        //Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = ["word",],
            OrWords = [],
            NotWords = ["word",],
        };
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var expected = new HashSet<string>(_originalExpectedResult);
        _searchExecutor.Construct(invertedIndex);
        _searcher.AndNotSearch(invertedIndex, words).Returns(expected);

        //Act
        var result = _searchExecutor.ExecuteSearch(words);

        //Assert
        result.Should().BeEquivalentTo(expected);
        _searcher.Received(1).AndNotSearch(invertedIndex, words);

        expected.Should().BeEquivalentTo(_originalExpectedResult);
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
    }

    [Fact]
    public void ExecuteSearch_WhenOrWordsPresent_ShouldCallOrNotSearchAndShouldNotModifyInputAndReturnValues()
    {
        //Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = ["word",],
            NotWords = ["word",],
        };
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var expected = new HashSet<string>(_originalExpectedResult);
        _searchExecutor.Construct(invertedIndex);
        _searcher.OrNotSearch(invertedIndex, words).Returns(expected);

        //Act
        var result = _searchExecutor.ExecuteSearch(words);

        //Assert
        result.Should().BeEquivalentTo(expected);
        _searcher.Received(1).OrNotSearch(invertedIndex, words);

        expected.Should().BeEquivalentTo(_originalExpectedResult);
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
    }

    [Fact]
    public void ExecuteSearch_WhenNoWordsArePresent_ShouldReturnEmptySetAndShouldNotModifyInputAndReturnValues()
    {
        //Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = [],
            NotWords = ["word",],
        };
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        _searchExecutor.Construct(invertedIndex);

        //Act
        var result = _searchExecutor.ExecuteSearch(words);

        //Assert
        _searcher.Received(0).AndOrNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        _searcher.Received(0).AndNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        _searcher.Received(0).OrNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        result.Should().BeEmpty();
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
    }

    [Fact]
    public void Constructor_WhenDependenciesAreNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISearcher searcher = null!;

        // Act & Assert
        Action act = () => new SearchExecutor(searcher);
        act.Should().Throw<ArgumentNullException>();
    }
}
