using FluentAssertions;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Application.WordsProcessors.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.WordProcessors;

public class WordsProcessorTests
{
    private readonly IAndWordsProcessor    _andWordsProcessor;
    private readonly IReadOnlyList<string> _expectedResult;
    private readonly INotWordsProcessor    _notWordsProcessor;
    private readonly IOrWordsProcessor     _orWordsProcessor;
    
    private readonly IReadOnlyList<string> _query;
    private readonly WordsProcessor        _wordsProcessor;
    
    public WordsProcessorTests()
    {
        _andWordsProcessor = Substitute.For<IAndWordsProcessor>();
        _orWordsProcessor = Substitute.For<IOrWordsProcessor>();
        _notWordsProcessor = Substitute.For<INotWordsProcessor>();
        _wordsProcessor = new(_andWordsProcessor, _orWordsProcessor, _notWordsProcessor);
        
        _query = ["word1", "word2",];
        _expectedResult = ["word11", "word12",];
    }
    
    [Fact]
    public void GetAndWords_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        _andWordsProcessor.GetAndWords(_query).Returns(_expectedResult);
        
        // Act
        var result = _wordsProcessor.GetAndWords(_query);
        
        // Assert
        result.Should().BeSameAs(_expectedResult);
        _andWordsProcessor.Received(1).GetAndWords(_query);
    }
    
    [Fact]
    public void GetOrWords_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        _orWordsProcessor.GetOrWords(_query).Returns(_expectedResult);
        
        // Act
        var result = _wordsProcessor.GetOrWords(_query);
        
        // Assert
        result.Should().BeSameAs(_expectedResult);
        _orWordsProcessor.Received(1).GetOrWords(_query);
    }
    
    [Fact]
    public void GetNotWords_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        _notWordsProcessor.GetNotWords(_query).Returns(_expectedResult);
        
        // Act
        var result = _wordsProcessor.GetNotWords(_query);
        
        // Assert
        result.Should().BeSameAs(_expectedResult);
        _notWordsProcessor.Received(1).GetNotWords(_query);
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var andWordsProcessor = Substitute.For<IAndWordsProcessor>();
        var orWordsProcessor = Substitute.For<IOrWordsProcessor>();
        var notWordsProcessor = Substitute.For<INotWordsProcessor>();
        
        // Act
        Action act1 = () => new WordsProcessor(null!, orWordsProcessor, notWordsProcessor);
        Action act2 = () => new WordsProcessor(andWordsProcessor, null!, notWordsProcessor);
        Action act3 = () => new WordsProcessor(andWordsProcessor, orWordsProcessor, null!);
        
        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
