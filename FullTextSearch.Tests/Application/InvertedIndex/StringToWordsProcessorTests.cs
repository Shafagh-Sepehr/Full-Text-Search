using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Services;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Abstractions;
using FullTextSearch.Application.StringCleaners.StringTrimAndSplitter.Abstractions;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class StringToWordsProcessorTests
{
    private readonly IStringListCleaner             _stringListCleaner;
    private readonly IStringListNoiseCleaner        _stringListNoiseCleaner;
    private readonly IStringListNonValidWordCleaner _stringListNonValidWordCleaner;
    private readonly IStringListStemmer             _stringListStemmer;
    private readonly StringToWordsProcessor         _stringToWordsProcessor;
    private readonly IStringTrimAndSplitter         _stringTrimAndSplitter;


    public StringToWordsProcessorTests()
    {
        _stringListNoiseCleaner = Substitute.For<IStringListNoiseCleaner>();
        _stringTrimAndSplitter = Substitute.For<IStringTrimAndSplitter>();
        _stringListCleaner = Substitute.For<IStringListCleaner>();
        _stringListStemmer = Substitute.For<IStringListStemmer>();
        _stringListNonValidWordCleaner = Substitute.For<IStringListNonValidWordCleaner>();

        _stringToWordsProcessor = new(_stringListNoiseCleaner, _stringTrimAndSplitter, _stringListCleaner, _stringListStemmer,
                                      _stringListNonValidWordCleaner);
    }

    [Fact]
    public void TrimSplitAndStemString_WhenCorrectlyCalled_ShouldRunMethodsInOrderAndShouldNotModifyInputOrReturnValues()
    {
        // Arrange
        const string source = "random string";
        var list1 = new List<string> { "str1", "str2", "str3", "str4", "str5", "str5", };
        var list2 = new List<string> { "strr1", "strr2", "strr3", "strr5", "strr5", };
        var list3 = new List<string> { "strrr1", "strrr2", "strrr3", "strrr4", };
        var list4 = new List<string> { "strrrrr1", "strrrrr3", "strrrrr3", "strrrrr3", };
        var list5 = new List<string> { "str1", "str2", "strrrrr2", "strrrrr2", "strrrrr2", };
        var expectedResult = new List<string> { "str1", "str2", "strrrrr2", };

        var sourceCopy = new string(source);
        var list1Copy = new List<string>(list1);
        var list2Copy = new List<string>(list2);
        var list3Copy = new List<string>(list3);
        var list4Copy = new List<string>(list4);
        var list5Copy = new List<string>(list5);
        var expectedResultCopy = new List<string>(expectedResult);

        _stringTrimAndSplitter.TrimAndSplit(source).Returns(list1);
        _stringListNoiseCleaner.CleanNoise(list1).Returns(list2);
        _stringListCleaner.Clean(list2).Returns(list3);
        _stringListStemmer.Stem(list3).Returns(list4);
        _stringListNonValidWordCleaner.Clean(list4).Returns(list5);

        // Act
        var result = _stringToWordsProcessor.TrimSplitAndStemString(source).ToList();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);

        Received.InOrder(() =>
        {
            _stringTrimAndSplitter.TrimAndSplit(source);
            _stringListNoiseCleaner.CleanNoise(list1);
            _stringListCleaner.Clean(list2);
            _stringListStemmer.Stem(list3);
            _stringListNonValidWordCleaner.Clean(list4);
        });

        sourceCopy.Should().BeEquivalentTo(source);
        list1Copy.Should().BeEquivalentTo(list1);
        list2Copy.Should().BeEquivalentTo(list2);
        list3Copy.Should().BeEquivalentTo(list3);
        list4Copy.Should().BeEquivalentTo(list4);
        list5Copy.Should().BeEquivalentTo(list5);
        result.Should().BeEquivalentTo(expectedResultCopy);
    }


    [Fact]
    public void TrimSplitAndStemString_WhenCorrectlyCalled_ShouldReturnListWithUniqueValues()
    {
        // Arrange
        var list = new List<string> { "str1", "str2", "str2", "strrrrr2", "strrrrr2", "strrrrr2", };
        var expectedResult = new List<string> { "str1", "str2", "strrrrr2", };

        _stringListNonValidWordCleaner.Clean(Arg.Any<IEnumerable<string>>()).Returns(list);

        // Act
        var result = _stringToWordsProcessor.TrimSplitAndStemString(null!).ToList();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Construct_WhenInputIsNotNull_ShouldCallConstructOnStringListNonValidWordCleaner()
    {
        // Arrange
        var input = new List<string> { "bannedWord", "bannedWordTwo", };
        var inputCopy = new List<string>(input);

        // Act
        _stringToWordsProcessor.Construct(input);

        // Assert
        _stringListNonValidWordCleaner.Received(1).Construct(input);
        inputCopy.Should().BeEquivalentTo(input);
    }
    
    [Fact]
    public void Construct_WhenInputNull_ShouldNotCallConstructOnStringListNonValidWordCleaner()
    {
        // Arrange
        List<string>? input = null;

        // Act
        _stringToWordsProcessor.Construct(input);

        // Assert
        _stringListNonValidWordCleaner.Received(0).Construct(input);
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var stringListNoiseCleaner = Substitute.For<IStringListNoiseCleaner>();
        var stringTrimAndSplitter = Substitute.For<IStringTrimAndSplitter>();
        var stringListCleaner = Substitute.For<IStringListCleaner>();
        var stringListStemmer = Substitute.For<IStringListStemmer>();
        var stringListNonValidWordCleaner = Substitute.For<IStringListNonValidWordCleaner>();

        // Act
        Action act1 = () => new StringToWordsProcessor(null!, stringTrimAndSplitter, stringListCleaner, stringListStemmer, stringListNonValidWordCleaner);
        Action act2 = () => new StringToWordsProcessor(stringListNoiseCleaner, null!, stringListCleaner, stringListStemmer, stringListNonValidWordCleaner);
        Action act3 = () => new StringToWordsProcessor(stringListNoiseCleaner, stringTrimAndSplitter, null!, stringListStemmer, stringListNonValidWordCleaner);
        Action act4 = () => new StringToWordsProcessor(stringListNoiseCleaner, stringTrimAndSplitter, stringListCleaner, null!, stringListNonValidWordCleaner);
        Action act5 = () => new StringToWordsProcessor(stringListNoiseCleaner, stringTrimAndSplitter, stringListCleaner, stringListStemmer, null!);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
        act4.Should().Throw<ArgumentNullException>();
        act5.Should().Throw<ArgumentNullException>();
    }
}
