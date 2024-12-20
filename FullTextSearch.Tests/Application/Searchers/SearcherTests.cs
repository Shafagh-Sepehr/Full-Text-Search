using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.DocumentsReader.Services;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Abstractions;
using FullTextSearch.Application.Searchers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.Searchers;

public class SearcherTests
{
    private readonly Searcher          _searcher;
    private readonly IAndOrNotSearcher _andOrNotSearcher;
    private readonly IAndNotSearcher   _andNotSearcher;
    private readonly IOrNotSearcher    _orNotSearcher;

    private readonly IReadOnlyDictionary<string, List<string>> _invertedIndex;
    private readonly ProcessedQueryWords                       _words;
    private readonly IReadOnlySet<string>                      _expectedResult;

    public SearcherTests()
    {
        _andOrNotSearcher = Substitute.For<IAndOrNotSearcher>();
        _andNotSearcher = Substitute.For<IAndNotSearcher>();
        _orNotSearcher = Substitute.For<IOrNotSearcher>();
        _searcher = new(_andOrNotSearcher, _andNotSearcher, _orNotSearcher);

        _invertedIndex = new Dictionary<string, List<string>>
        {
            { "word1", ["doc1", "doc2",] },
            { "word2", ["doc3",] },
        };
        _words = new()
        {
            AndWords = ["andword1", "andword2",],
            OrWords = ["orword1", "orword2",],
            NotWords = ["notword1", "notword2",],
        };
        _expectedResult = new HashSet<string> { "res1", "res2", "res3", };
    }

    [Fact]
    public void AndOrNotSearch_WhenCorrectlyCalled_ShouldCallAndOrNotSearchCorrectlyAndReturnItsValueUnchanged()
    {
        // Arrange
        var expectedResult = new HashSet<string>(_expectedResult);
        _andOrNotSearcher.AndOrNotSearch(_invertedIndex, _words).Returns(expectedResult);
        
        // Act
        var result = _searcher.AndOrNotSearch(_invertedIndex, _words);
        
        // Assert
        result.Should().BeSameAs(expectedResult);
        _andOrNotSearcher.Received(1).AndOrNotSearch(_invertedIndex, _words);
    }

    [Fact]
    public void AndNotSearch_WhenCorrectlyCalled_ShouldCallAndNotSearchCorrectlyAndReturnItsValueUnchanged()
    {
        // Arrange
        var expectedResult = new HashSet<string>(_expectedResult);
        _andNotSearcher.AndNotSearch(_invertedIndex, _words).Returns(expectedResult);
        
        // Act
        var result = _searcher.AndNotSearch(_invertedIndex, _words);
        
        // Assert
        result.Should().BeSameAs(expectedResult);
        _andNotSearcher.Received(1).AndNotSearch(_invertedIndex, _words);
    }

    [Fact]
    public void OrNotSearch_WhenCorrectlyCalled_ShouldCallOrNotSearchCorrectlyAndReturnItsValueUnchanged()
    {
        // Arrange
        var expectedResult = new HashSet<string>(_expectedResult);
        _orNotSearcher.OrNotSearch(_invertedIndex, _words).Returns(expectedResult);
        
        // Act
        var result = _searcher.OrNotSearch(_invertedIndex, _words);

        // Assert
        result.Should().BeSameAs(expectedResult);
        _orNotSearcher.Received(1).OrNotSearch(_invertedIndex, _words);
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var andOrNotSearcher = Substitute.For<IAndOrNotSearcher>();
        var andNotSearcher = Substitute.For<IAndNotSearcher>();
        var orNotSearcher = Substitute.For<IOrNotSearcher>();

        // Act
        Action act1 = () => new Searcher(null!, andNotSearcher, orNotSearcher);
        Action act2 = () => new Searcher(andOrNotSearcher, null!, orNotSearcher);
        Action act3 = () => new Searcher(andOrNotSearcher, andNotSearcher, null!);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
