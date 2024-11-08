using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.Searchers;

public class OrNotSearcherTests
{
    private readonly OrNotSearcher   _andOrNotSearcher;
    private readonly IDocumentReader _documentReader;
    
    
    public OrNotSearcherTests()
    {
        _documentReader = Substitute.For<IDocumentReader>();
        _andOrNotSearcher = new(_documentReader);
    }
    
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void OrNotSearch_WhenCorrectlyCalled_ShouldCallMethodsInOrderAndShouldReturnCorrectDocs(
        HashSet<string> orDocs, HashSet<string> notDocs, IReadOnlySet<string> expectedResult)
    {
        // Arrange
        IReadOnlyDictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();
        var words = new ProcessedQueryWords
        {
            AndWords = [],
            OrWords = ["orword1", "orword2",],
            NotWords = ["notword1", "notword2",],
        };
        
        _documentReader.GetOrDocuments(invertedIndex, words.OrWords).Returns(orDocs);
        _documentReader.GetNotDocuments(invertedIndex, words.NotWords).Returns(notDocs);
        
        // Act
        var result = _andOrNotSearcher.OrNotSearch(invertedIndex, words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        Received.InOrder(() =>
        {
            _documentReader.GetOrDocuments(invertedIndex, words.OrWords);
            _documentReader.GetNotDocuments(invertedIndex, words.NotWords);
        });
        _documentReader.DidNotReceiveWithAnyArgs().GetAndDocuments(default!, default!);
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
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IDocumentReader documentReader = null!;
        
        // Act
        Action act = () => new OrNotSearcher(documentReader);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
