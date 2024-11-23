using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class InvertedIndexDictionaryTests
{
    private readonly InvertedIndexDictionary        _invertedIndexDictionary;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller;
    private readonly IAppSettings                   _appSettings;
    private readonly IQuerySearcher                 _querySearcher;
    
    public InvertedIndexDictionaryTests()
    {
        _querySearcher = Substitute.For<IQuerySearcher>();
        _invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();
        _appSettings = Substitute.For<IAppSettings>();
        _invertedIndexDictionary = new(_querySearcher, _invertedIndexDictionaryFiller, _appSettings);
    }

    
    [Fact]
    public void Search_WhenCorrectlyCalled_ShouldCallSearchOnSearcherReturnItsReturnValue()
    {
        // Arrange
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
        var appSettings = Substitute.For<IAppSettings>();

        // Act
        Action act1 = () => new InvertedIndexDictionary(null!, invertedIndexDictionaryFiller,appSettings);
        Action act2 = () => new InvertedIndexDictionary(querySearcher, null!,appSettings);
        Action act3 = () => new InvertedIndexDictionary(querySearcher, invertedIndexDictionaryFiller,null!);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
