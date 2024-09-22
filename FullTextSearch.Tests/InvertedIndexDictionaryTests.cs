

using System.Text.Json;
using System.Threading.Tasks.Sources;
using CodeStar2;
using CodeStar2.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace FullTextSearch.Tests;

public class InvertedIndexDictionaryTests
{

    private readonly Dictionary<string, List<string>>? _testInvertedIndex =
        JsonSerializer.Deserialize<Dictionary<string, List<string>>>(InvertedIndexDictionaryBuilderTestData.InvertedIndexJson);

    private readonly IQuerySearcher                  _querySearcher          = Substitute.For<IQuerySearcher>();
    private readonly IInvertedIndexDictionaryBuilder _indexDictionaryBuilder = Substitute.For<IInvertedIndexDictionaryBuilder>();

    [Theory]
    [MemberData(nameof(TestData))]
    public void InvertedIndexSearchTest(string query, string[] expectedResult)
    {
        //pre Assert
        _testInvertedIndex.Should().NotBeNull();
        
        //Arrange
        _indexDictionaryBuilder.Build("/file/path").Returns(_testInvertedIndex);
        _querySearcher.Search(query, _testInvertedIndex!).Returns(expectedResult);
        
        var invertedIndexDictionary = new InvertedIndexDictionary("/file/path", _indexDictionaryBuilder, _querySearcher);

        //Act
        string[] result = invertedIndexDictionary.Search(query).ToArray();
        
        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    
    public static IEnumerable<object?[]> TestData()
    {
        yield return ["doesn't matter what this is", new[] { "doesn't matter","what this is"}, ];
    }
}
