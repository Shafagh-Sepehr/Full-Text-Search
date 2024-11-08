using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.WordsProcessors.Abstractions;
using FullTextSearch.Exceptions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class QuerySearcherTests
{
    private readonly QuerySearcher   _querySearcher;
    private readonly ISearchExecutor _searchExecutor;
    private readonly IWordsProcessor _wordsProcessor;
    
    public QuerySearcherTests()
    {
        _wordsProcessor = Substitute.For<IWordsProcessor>();
        _searchExecutor = Substitute.For<ISearchExecutor>();
        _querySearcher = new(_wordsProcessor, _searchExecutor);
    }
    
    [Fact]
    public void Construct_WhenCorrectlyCalled_ShouldCallConstructMethodOnQuerySearcher()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>();
        
        // Act
        _querySearcher.Construct(invertedIndex);
        
        // Assert
        _searchExecutor.Received(1).Construct(invertedIndex);
    }
    
    [Fact]
    public void Search_WhenConstructIsNotCalled_ShouldThrowException()
    {
        // Act & Assert
        Action act = () => _querySearcher.Search("some query");
        act.Should().Throw<ConstructMethodNotCalledException>();
    }
    
    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceTestData))]
    public void Search_WhenQueryIsNullOrWhiteSpace_ShouldReturnEmptyList(string query)
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>();
        _querySearcher.Construct(invertedIndex);
        
        // Act
        var result = _querySearcher.Search(query);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    public static IEnumerable<object?[]> NullOrWhiteSpaceTestData()
    {
        yield return ["",];
        yield return [null,];
        yield return ["   ",];
    }
    
    [Fact]
    public void Search_WhenCorrectlyCalled_ShouldReturnExpectedResult()
    {
        // Arrange
        const string query = "  query words are here ";
        var processedQuery = query.Trim().Split();
        var invertedIndex = new Dictionary<string, List<string>>
        {
            { "key1", ["value1", "value2",] },
            { "key2", ["value3", "value4",] },
            { "key3", ["value5", "value6",] },
        };
        
        var andReturns = new List<string> { "key1", "keey1", };
        var orReturns = new List<string> { "key2", "keey2", };
        var notReturns = new List<string> { "key3", "keey3", };
        
        _querySearcher.Construct(invertedIndex);
        
        _wordsProcessor.GetOrWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any())).Returns(orReturns);
        _wordsProcessor.GetAndWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any())).Returns(andReturns);
        _wordsProcessor.GetNotWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any())).Returns(notReturns);
        
        var expectedResult = new HashSet<string> { "res1", "res2", };
        var expectedResultCopy = new HashSet<string>(expectedResult);
        
        
        _searchExecutor.ExecuteSearch(Arg.Is<ProcessedQueryWords>(x =>
            x.AndWords.Count == andReturns.Count && !x.AndWords.Except(andReturns).Any() &&
            x.OrWords.Count == orReturns.Count && !x.OrWords.Except(orReturns).Any() &&
            x.NotWords.Count == notReturns.Count && !x.NotWords.Except(notReturns).Any())).Returns(expectedResult);
        
        // Act
        var result = _querySearcher.Search(query);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _wordsProcessor.Received(1).GetOrWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any()));
        _wordsProcessor.Received(1).GetAndWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any()));
        _wordsProcessor.Received(1).GetNotWords(Arg.Is<string[]>(x => x.Length == processedQuery.Length && !x.Except(processedQuery).Any()));
        _searchExecutor.Received(1).ExecuteSearch(Arg.Do<ProcessedQueryWords>(processedQueryWords =>
        {
            processedQueryWords.OrWords.Should().BeEquivalentTo(orReturns);
            processedQueryWords.AndWords.Should().BeEquivalentTo(andReturns);
            processedQueryWords.NotWords.Should().BeEquivalentTo(notReturns);
        }));
        expectedResult.Should().BeEquivalentTo(expectedResultCopy);
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var wordsProcessor = Substitute.For<IWordsProcessor>();
        var searcher = Substitute.For<ISearchExecutor>();
        
        // Act
        Action act1 = () => new QuerySearcher(null!, searcher);
        Action act2 = () => new QuerySearcher(wordsProcessor, null!);
        
        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
    }
}
