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
    private readonly IAndOrNotSearcher _andOrNotSearcher;
    private readonly IAndNotSearcher   _andNotSearcher;
    private readonly IOrNotSearcher    _orNotSearcher;
    private readonly Searcher          _searcher;

    
    private readonly Dictionary<string, List<string>> _originalInvertedIndex;
    private readonly QueryProcessedWords              _originalWords;
    private readonly HashSet<string>                  _originalExpectedResult;

    public SearcherTests()
    {
        _andOrNotSearcher = Substitute.For<IAndOrNotSearcher>();
        _andNotSearcher = Substitute.For<IAndNotSearcher>();
        _orNotSearcher = Substitute.For<IOrNotSearcher>();
        _searcher = new(_andOrNotSearcher,_andNotSearcher,_orNotSearcher);

        _originalInvertedIndex = new()
        {
            { "word1", ["doc1", "doc2",] },
            { "word2", ["doc3",] },
        };
        _originalWords = new()
        {
            AndWords = ["andword1", "andword2",],
            OrWords = ["orword1", "orword2",],
            NotWords = ["notword1", "notword2",],
        };
        _originalExpectedResult = ["res1","res2","res3"];
    }

    [Fact]
    public void AndOrNotSearch_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var words = new QueryProcessedWords
        {
            AndWords = _originalWords.AndWords,
            OrWords = _originalWords.OrWords,
            NotWords = _originalWords.NotWords,
        };
        var expectedResult = new HashSet<string>(_originalExpectedResult);
        
        _andOrNotSearcher.AndOrNotSearch(invertedIndex, words).Returns(expectedResult);
        
        // Act
        var result = _searcher.AndOrNotSearch(invertedIndex, words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _andOrNotSearcher.Received(1).AndOrNotSearch(invertedIndex, words); 
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        words.Should().BeEquivalentTo(_originalWords);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void AndNotSearch_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var words = new QueryProcessedWords
        {
            AndWords = _originalWords.AndWords,
            OrWords = _originalWords.OrWords,
            NotWords = _originalWords.NotWords,
        };
        var expectedResult = new HashSet<string>(_originalExpectedResult);
        
        _andNotSearcher.AndNotSearch(invertedIndex, words).Returns(expectedResult);
        
        // Act
        var result = _searcher.AndNotSearch(invertedIndex, words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _andNotSearcher.Received(1).AndNotSearch(invertedIndex, words);
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        words.Should().BeEquivalentTo(_originalWords);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void OrNotSearch_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var words = new QueryProcessedWords
        {
            AndWords = _originalWords.AndWords,
            OrWords = _originalWords.OrWords,
            NotWords = _originalWords.NotWords,
        };
        var expectedResult = new HashSet<string>(_originalExpectedResult);
        
        _orNotSearcher.OrNotSearch(invertedIndex, words).Returns(expectedResult);
        
        // Act
        var result = _searcher.OrNotSearch(invertedIndex, words);
        
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _orNotSearcher.Received(1).OrNotSearch(invertedIndex, words);
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        words.Should().BeEquivalentTo(_originalWords);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IAndDocumentsReader nullAndReader = null!;
        IOrDocumentsReader nullOrReader = null!;
        INotDocumentsReader nullNotReader = null!;

        // Act & Assert
        Action act = () => new DocumentReader(nullAndReader, nullOrReader, nullNotReader);
        act.Should().Throw<ArgumentNullException>();
    }
}