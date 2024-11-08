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

    private readonly Dictionary<string, List<string>> _invertedIndex  = new() { { "key", ["val1", "val2",] }, };
    private readonly HashSet<string>                  _expectedResult = ["words",];

    public SearchExecutorTests()
    {
        _searcher = Substitute.For<ISearcher>();
        _searchExecutor = new(_searcher);
    }

    [Fact]
    public void ExecuteSearch_WhenConstructMethodIsNotCalled_ShouldThrowException()
    {
        // Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = [],
            NotWords = [],
        };

        // Act
        Action act = () => _searchExecutor.ExecuteSearch(words);

        // Assert
        act.Should().Throw<ConstructMethodNotCalledException>();
    }

    [Fact]
    public void ExecuteSearch_WhenAllWordsPresent_ShouldCallAndOrNotSearchWithCorrectInputs()
    {
        // Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = ["word",],
            OrWords = ["word",],
            NotWords = ["word",],
        };
        _searchExecutor.Construct(_invertedIndex);
        _searcher.AndOrNotSearch(_invertedIndex, words).Returns(_expectedResult);

        // Act
        var result = _searchExecutor.ExecuteSearch(words);

        // Assert
        result.Should().BeSameAs(_expectedResult);
        _searcher.Received(1).AndOrNotSearch(_invertedIndex, words);
    }

    [Fact]
    public void ExecuteSearch_WhenAndWordsPresent_ShouldCallAndNotSearchWithCorrectInputs()
    {
        // Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = ["word",],
            OrWords = [],
            NotWords = ["word",],
        };
        _searchExecutor.Construct(_invertedIndex);
        _searcher.AndNotSearch(_invertedIndex, words).Returns(_expectedResult);

        // Act
        var result = _searchExecutor.ExecuteSearch(words);

        // Assert
        result.Should().BeSameAs(_expectedResult);
        _searcher.Received(1).AndNotSearch(_invertedIndex, words);
    }

    [Fact]
    public void ExecuteSearch_WhenOrWordsPresent_ShouldCallOrNotSearchWithCorrectInputs()
    {
        // Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = ["word",],
            NotWords = ["word",],
        };
        _searchExecutor.Construct(_invertedIndex);
        _searcher.OrNotSearch(_invertedIndex, words).Returns(_expectedResult);

        // Act
        var result = _searchExecutor.ExecuteSearch(words);

        // Assert
        result.Should().BeSameAs(_expectedResult);
        _searcher.Received(1).OrNotSearch(_invertedIndex, words);
    }

    [Fact]
    public void ExecuteSearch_WhenNoWordsArePresent_ShouldReturnEmptySetAndShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = [],
            NotWords = ["word",],
        };
        _searchExecutor.Construct(_invertedIndex);

        // Act
        var result = _searchExecutor.ExecuteSearch(words);

        // Assert
        _searcher.Received(0).AndOrNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        _searcher.Received(0).AndNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        _searcher.Received(0).OrNotSearch(Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<ProcessedQueryWords>());
        result.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ISearcher searcher = null!;

        // Act
        Action act = () => new SearchExecutor(searcher);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
