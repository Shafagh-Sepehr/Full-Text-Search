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
    private readonly ISearchExecutor _searchExecutor;
    private readonly IWordsProcessor _wordsProcessor;
    private readonly QuerySearcher   _querySearcher;
    
    public QuerySearcherTests()
    {
        _wordsProcessor = Substitute.For<IWordsProcessor>();
        _searchExecutor = Substitute.For<ISearchExecutor>();
        _querySearcher = new(_wordsProcessor, _searchExecutor);
    }

    [Fact]
    public void Construct_ShouldNotModifyInputValue()
    {
        //Arrange
        var invertedIndex = new Dictionary<string, List<string>>
        {
            { "key1", ["value1", "value2",] },
            { "key2", ["value3", "value4",] },
        };
        var invertedIndexCopy = new Dictionary<string, List<string>>(invertedIndex);

        //Act
        _querySearcher.Construct(invertedIndex);

        //Assert
        _searchExecutor.Received(1).Construct(invertedIndex);
        invertedIndexCopy.Should().BeEquivalentTo(invertedIndex);
    }
    
    [Fact]
    public void Search_ThrowsException_IfConstructIsNotCalled()
    {
        // Act & Assert
        Action act = () => _querySearcher.Search("some query");
        act.Should().Throw<ConstructMethodNotCalledException>();
    }
    
    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceTestData))]
    public void Search_ReturnsEmptyList_IfQueryIsNullOrWhiteSpace(string query)
    {
        //Arrange
        var invertedIndex = new Dictionary<string, List<string>>();
        _querySearcher.Construct(invertedIndex);
        
        //Act
        var result = _querySearcher.Search(query);
        
        //Assert
        result.Should().BeEmpty();
    }

    public static IEnumerable<object?[]> NullOrWhiteSpaceTestData()
    {
        yield return ["",];
        yield return [null,];
        yield return ["   ",];
    }
    
    [Fact]
    public void Search_ReturnsList()
    {
        //Arrange
        var query = "  query words are here ";
        var processedQuery = "  query words are here ".Trim().Split();
        var invertedIndex = new Dictionary<string, List<string>>
        {
            { "key1", ["value1", "value2",] },
            { "key2", ["value3", "value4",] },
            { "key3", ["value5", "value6",] },
        };

        _querySearcher.Construct(invertedIndex);

        _wordsProcessor.GetOrWords(processedQuery).Returns(["key1", "keey1",]);
        _wordsProcessor.GetAndWords(processedQuery).Returns(["key2", "keey2",]);
        _wordsProcessor.GetNotWords(processedQuery).Returns(["key3", "keey3",]);

        var expectedResult = new HashSet<string> { "res1", "res2", };
        var expectedResultCopy = new HashSet<string>(expectedResult);

        var processedWords = new ProcessedQueryWords
        {
            OrWords = _wordsProcessor.GetOrWords(processedQuery),
            AndWords = _wordsProcessor.GetAndWords(processedQuery),
            NotWords = _wordsProcessor.GetNotWords(processedQuery),
        };

        _searchExecutor.ExecuteSearch(processedWords).Returns(expectedResult);

        //Act
        var result = _querySearcher.Search(query);

        //Assert
        result.Should().BeEmpty();
        _wordsProcessor.Received(1).GetOrWords(processedQuery);
        _wordsProcessor.Received(1).GetAndWords(processedQuery);
        _wordsProcessor.Received(1).GetNotWords(processedQuery);
        _searchExecutor.Received(1).ExecuteSearch(Arg.Do<ProcessedQueryWords>(processedQueryWords =>
        {
            processedQueryWords.OrWords.Should().BeEquivalentTo(processedWords.OrWords);
            processedQueryWords.AndWords.Should().BeEquivalentTo(processedWords.AndWords);
            processedQueryWords.NotWords.Should().BeEquivalentTo(processedWords.NotWords);
        }));
        expectedResult.Should().BeEquivalentTo(expectedResultCopy);
    }
    
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IWordsProcessor wordsProcessor = null!;
        ISearchExecutor searcher = null!;
        
        // Act & Assert
        Action act = () => new QuerySearcher(wordsProcessor, searcher);
        act.Should().Throw<ArgumentNullException>();
    }
}
