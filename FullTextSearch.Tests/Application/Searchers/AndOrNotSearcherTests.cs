using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.Models;
using FullTextSearch.Application.Searchers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.Searchers;

public class AndOrNotSearcherTests
{
    private readonly AndOrNotSearcher _andOrNotSearcher;
    private readonly IDocumentReader  _documentReader;
    
    private readonly ProcessedQueryWords _words;
    
    public AndOrNotSearcherTests()
    {
        _documentReader = Substitute.For<IDocumentReader>();
        _andOrNotSearcher = new(_documentReader);
        
        _words = new()
        {
            AndWords = ["andword1", "andword2",],
            OrWords = ["orword1", "orword2",],
            NotWords = ["notword1", "notword2",],
        };
    }
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void AndOrNotSearch_WhenCorrectlyCalled_ShouldCallMethodsInOrderAndShouldReturnCorrectDocs(
        HashSet<string> andDocs, HashSet<string> orDocs, HashSet<string> notDocs, IReadOnlySet<string> expectedResult)
    {
        // Arrange
        IReadOnlyDictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();
        var words = new ProcessedQueryWords
        {
            AndWords = ["andword1", "andword2",],
            OrWords = ["orword1", "orword2",],
            NotWords = ["notword1", "notword2",],
        };
        
        _documentReader.GetAndDocuments(invertedIndex, words.AndWords).Returns(andDocs);
        _documentReader.GetOrDocuments(invertedIndex, words.OrWords).Returns(orDocs);
        _documentReader.GetNotDocuments(invertedIndex, words.NotWords).Returns(notDocs);
        
        // Act
        var result = _andOrNotSearcher.AndOrNotSearch(invertedIndex, words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        Received.InOrder(() =>
        {
            _documentReader.GetAndDocuments(invertedIndex, words.AndWords);
            _documentReader.GetOrDocuments(invertedIndex, words.OrWords);
            _documentReader.GetNotDocuments(invertedIndex, words.NotWords);
        });
    }
    
    public static IEnumerable<object?[]> TestData()
    {
        yield return [new HashSet<string> { "1", }, new HashSet<string> { "2", }, new HashSet<string>(), new HashSet<string>(),];
        yield return [new HashSet<string> { "1", }, new HashSet<string> { "1", }, new HashSet<string>(), new HashSet<string> { "1", },];
        yield return [new HashSet<string> { "1", }, new HashSet<string> { "1", }, new HashSet<string> { "1", }, new HashSet<string>(),];
        yield return [new HashSet<string> { "1", }, new HashSet<string> { "1", }, new HashSet<string> { "2", }, new HashSet<string> { "1", },];
        yield return [new HashSet<string> { "1", }, new HashSet<string> { "1", }, new HashSet<string> { "2", }, new HashSet<string> { "1", },];
        yield return
        [
            new HashSet<string> { "1", "2", "3", }, new HashSet<string> { "1", "2", "4", }, new HashSet<string> { "2", },
            new HashSet<string> { "1", },
        ];
        yield return
        [
            new HashSet<string> { "1", "2", "3", }, new HashSet<string> { "1", "2", "4", }, new HashSet<string>(),
            new HashSet<string> { "1", "2", },
        ];
        yield return
        [
            new HashSet<string> { "1", "2", "3", "4", "6", "8", }, new HashSet<string> { "1", "2", "4", "5", "6", "7", },
            new HashSet<string> { "1", "2", }, new HashSet<string> { "4", "6", },
        ];
        yield return
        [
            new HashSet<string> { "1", "2", "3", "4", "6", "8", }, new HashSet<string> { "1", "2", "4", "5", "6", "7", },
            new HashSet<string> { "1", "2", "4", "6", "7", "8", }, new HashSet<string>(),
        ];
        yield return
        [
            new HashSet<string> { "1", "2", "3", "4", "6", "8", }, new HashSet<string> { "1", "2", "4", "5", "6", "7", }, new HashSet<string>(),
            new HashSet<string> { "1", "2", "4", "6", },
        ];
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IDocumentReader documentReader = null!;
        
        // Act
        Action act = () => new AndOrNotSearcher(documentReader);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
