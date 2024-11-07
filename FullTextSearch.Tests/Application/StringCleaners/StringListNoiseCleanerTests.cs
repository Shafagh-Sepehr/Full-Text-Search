using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.StringCleaners.StringListNoiseCleaner.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.StringCleaners;

public class StringListNoiseCleanerTests
{
    private readonly IRegexChecker          _regexChecker;
    private readonly StringListNoiseCleaner _stringListCleaner;


    public StringListNoiseCleanerTests()
    {
        _regexChecker = Substitute.For<IRegexChecker>();
        _stringListCleaner = new(_regexChecker);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void CleanNoise_WhenCorrectlyCalled_ShouldReturnResultThatAreNotMatchedByRegexPatternsAndShouldNotModifyInputValues(
        bool hasUrl, bool hasEmail, bool hasPhoneNumber, bool expectedResult)
    {
        // Arrange
        const string input = "abcd";
        List<string> inputList = [input,];
        List<string> inputListCopy = [..inputList,];
        _regexChecker.HasPhoneNumber(input).Returns(hasPhoneNumber);
        _regexChecker.HasEmail(input).Returns(hasEmail);
        _regexChecker.HasUrl(input).Returns(hasUrl);

        // Act
        var result = _stringListCleaner.CleanNoise(inputList);

        // Assert
        if (expectedResult)
            result.Should().BeEmpty();
        else
            result.Should().ContainSingle(input);
        inputList.Should().BeEquivalentTo(inputListCopy);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return [false, false, false, false,];
        yield return [false, false, true, true,];
        yield return [false, true, false, true,];
        yield return [false, true, true, true,];
        yield return [true, false, false, true,];
        yield return [true, false, true, true,];
        yield return [true, true, false, true,];
        yield return [true, true, true, true,];
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IRegexChecker regexChecker = null!;

        // Act
        Action act = () => new StringListNoiseCleaner(regexChecker);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
