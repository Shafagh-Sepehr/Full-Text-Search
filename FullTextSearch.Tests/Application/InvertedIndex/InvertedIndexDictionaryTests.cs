using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.ConfigurationService.Abstractions;
using FullTextSearch.Exceptions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class InvertedIndexDictionaryTests
{
    private readonly InvertedIndexDictionary        _invertedIndexDictionary;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller;
    private readonly IConfigurationService          _configurationService;
    private readonly IQuerySearcher                 _querySearcher;
    
    public InvertedIndexDictionaryTests()
    {
        _querySearcher = Substitute.For<IQuerySearcher>();
        _invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();
        _configurationService = Substitute.For<IConfigurationService>();
        _invertedIndexDictionary = new(_querySearcher, _invertedIndexDictionaryFiller,_configurationService);
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
        var configurationService = Substitute.For<IConfigurationService>();

        // Act
        Action act1 = () => new InvertedIndexDictionary(null!, invertedIndexDictionaryFiller,configurationService);
        Action act2 = () => new InvertedIndexDictionary(querySearcher, null!,configurationService);
        Action act3 = () => new InvertedIndexDictionary(querySearcher, invertedIndexDictionaryFiller,null!);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
