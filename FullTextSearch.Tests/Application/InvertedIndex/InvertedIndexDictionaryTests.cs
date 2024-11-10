using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Exceptions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class InvertedIndexDictionaryTests
{
    private readonly InvertedIndexDictionary        _invertedIndexDictionary;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller;
    private readonly IQuerySearcher                 _querySearcher;
    
    public InvertedIndexDictionaryTests()
    {
        _querySearcher = Substitute.For<IQuerySearcher>();
        _invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();
        _invertedIndexDictionary = new(_querySearcher, _invertedIndexDictionaryFiller);
    }
    
    [Fact]
    public void Construct_WhenCorrectlyCalled_ShouldCallMethodsInOrderWithCorrectInputs()
    {
        // Arrange
        const string path = "path";
        IReadOnlyList<string> bannedWords = new List<string> { "bannedWords", };
        IReadOnlyDictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();
        
        _invertedIndexDictionaryFiller.Build(path).Returns(invertedIndex);
        
        // Act
        _invertedIndexDictionary.Construct(path, bannedWords);
        
        // Assert
        _invertedIndexDictionaryFiller.Received(1).Construct(bannedWords);
        Received.InOrder(() =>
        {
            _invertedIndexDictionaryFiller.Build(path);
            _querySearcher.Construct(invertedIndex);
        });
    }
    
    
    [Fact]
    public void Search_WhenConstructMethodIsNotCalled_ShouldThrowException()
    {
        // Act
        Action act = () => _invertedIndexDictionary.Search("sth");
        
        // Assert
        act.Should().Throw<ConstructMethodNotCalledException>();
    }
    
    [Fact]
    public void Search_WhenCorrectlyCalled_ShouldCallSearchOnSearcherReturnItsReturnValue()
    {
        // Arrange
        _invertedIndexDictionary.Construct("path");
        IReadOnlySet<string> expectedResult = new HashSet<string> { "res", };
        
        const string query = "query";
        _querySearcher.Search(query).Returns(expectedResult);
        
        // Act
        var result = _invertedIndexDictionary.Search(query);
        
        // Assert
        result.Should().BeSameAs(expectedResult);
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var querySearcher = Substitute.For<IQuerySearcher>();
        var invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();
        
        // Act
        Action act1 = () => new InvertedIndexDictionary(null!, invertedIndexDictionaryFiller);
        Action act2 = () => new InvertedIndexDictionary(querySearcher, null!);
        
        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
    }
}
