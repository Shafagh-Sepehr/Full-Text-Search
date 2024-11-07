using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Exceptions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class InvertedIndexDictionaryTests
{
    private readonly IQuerySearcher                 _querySearcher;
    private readonly IInvertedIndexDictionaryFiller _invertedIndexDictionaryFiller;
    private readonly InvertedIndexDictionary        _invertedIndexDictionary;

    public InvertedIndexDictionaryTests()
    {
        _querySearcher = Substitute.For<IQuerySearcher>();
        _invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();
        _invertedIndexDictionary = new(_querySearcher, _invertedIndexDictionaryFiller);
    }

    [Fact]
    public void Construct_WhenCorrectlyCalled_ShouldCallInnerMethodsAndNotModifyInputAndReturnValues()
    {
        //Arrange
        const string path = "path";
        var pathCopy = new string(path);
        IEnumerable<string>? banned = new List<string> { "banned", };
        var bannedCopy = new List<string>(banned);
        var invertedIndex = new Dictionary<string, List<string>> { { "val", ["key",] }, };
        var invertedIndexCopy = new Dictionary<string, List<string>> (invertedIndex);

        _invertedIndexDictionaryFiller.Build(path).Returns(invertedIndex);

        //Act
        _invertedIndexDictionary.Construct(path,banned);
        //Assert

        _invertedIndexDictionaryFiller.Received(1).Construct(banned);
        _invertedIndexDictionaryFiller.Received(1).Build(path);
        _querySearcher.Received(1).Construct(invertedIndex);

        banned.Should().BeEquivalentTo(bannedCopy);
        invertedIndex.Should().BeEquivalentTo(invertedIndexCopy);
        path.Should().BeEquivalentTo(pathCopy);
    }


    [Fact]
    public void Search_WhenConstructMethodIsNotCalled_ShouldThrowException()
    {
        //Act
        Action act = () => _invertedIndexDictionary.Search("sth");

        //Assert
        act.Should().Throw<ConstructMethodNotCalledException>();
    }

    [Fact]
    public void Search_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        //Arrange
        const string path = "path";
        _invertedIndexDictionary.Construct(path);

        IReadOnlySet<string> expectedResult = new HashSet<string> { "res", };
        var expectedResultCopy = new List<string>(expectedResult);
        const string query = "query";
        var queryCopy = new string(query);

        _querySearcher.Search(query).Returns(expectedResult);

        //Act
        var result = _invertedIndexDictionary.Search(query).ToList();

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
        result.Should().BeEquivalentTo(expectedResultCopy);
        query.Should().BeEquivalentTo(queryCopy);
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var querySearcher = Substitute.For<IQuerySearcher>();
        var invertedIndexDictionaryFiller = Substitute.For<IInvertedIndexDictionaryFiller>();

        //Act
        Action act1 = () => new InvertedIndexDictionary(null!, invertedIndexDictionaryFiller);
        Action act2 = () => new InvertedIndexDictionary(querySearcher, null!);

        //Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
    }
}
