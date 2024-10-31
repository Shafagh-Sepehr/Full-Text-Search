using FluentAssertions;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Application.WordsProcessors.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.WordProcessors;

public class WordsProcessorTests
{
    private readonly IAndWordsProcessor _andWordsProcessor;
    private readonly IOrWordsProcessor  _orWordsProcessor;
    private readonly INotWordsProcessor _notWordsProcessor;
    private readonly WordsProcessor     _wordsProcessor;

    private readonly string[]              _originalQuery;
    private readonly IReadOnlyList<string> _originalExpectedResult;

    public WordsProcessorTests()
    {
        _andWordsProcessor = Substitute.For<IAndWordsProcessor>();
        _orWordsProcessor = Substitute.For<IOrWordsProcessor>();
        _notWordsProcessor = Substitute.For<INotWordsProcessor>();
        _wordsProcessor = new(_andWordsProcessor, _orWordsProcessor, _notWordsProcessor);

        _originalQuery = ["word1", "word2",];
        _originalExpectedResult = ["word11", "word12",];
    }

    [Fact]
    public void GetAndWords_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var query = new string[_originalQuery.Length];
        Array.Copy(_originalQuery,query,_originalQuery.Length);
        var expectedResult = new List<string>(_originalExpectedResult);
        
        
        _andWordsProcessor.GetAndWords(query).Returns(expectedResult);
        
        // Act
        var result = _wordsProcessor.GetAndWords(query);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _andWordsProcessor.Received(1).GetAndWords(query); 
        
        // Verify that the original inputs are unchanged
        query.Should().BeEquivalentTo(_originalQuery);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void GetOrWords_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var query = new string[_originalQuery.Length];
        Array.Copy(_originalQuery,query,_originalQuery.Length);
        var expectedResult = new List<string>(_originalExpectedResult);
        
        _orWordsProcessor.GetOrWords(query).Returns(expectedResult);
        
        // Act
        var result = _wordsProcessor.GetOrWords(query);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _orWordsProcessor.Received(1).GetOrWords(query);
        
        // Verify that the original inputs are unchanged
        query.Should().BeEquivalentTo(_originalQuery);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void GetNotWords_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var query = new string[_originalQuery.Length];
        Array.Copy(_originalQuery,query,_originalQuery.Length);
        var expectedResult = new List<string>(_originalExpectedResult);
        
        _notWordsProcessor.GetNotWords(query).Returns(expectedResult);
        
        // Act
        var result = _wordsProcessor.GetNotWords(query);
        
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _notWordsProcessor.Received(1).GetNotWords(query);
        
        // Verify that the original inputs are unchanged
        query.Should().BeEquivalentTo(_originalQuery);
        expectedResult.Should().BeEquivalentTo(_originalExpectedResult);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IAndWordsProcessor andWordsProcessor = null!;
        IOrWordsProcessor  orWordsProcessor  = null!;
        INotWordsProcessor notWordsProcessor = null!;

        // Act & Assert
        Action act = () => new WordsProcessor(andWordsProcessor, orWordsProcessor, notWordsProcessor);
        act.Should().Throw<ArgumentNullException>();
    }
}
