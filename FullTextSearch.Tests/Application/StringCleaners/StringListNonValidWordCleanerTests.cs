using FluentAssertions;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Services;

namespace FullTextSearch.Tests.Application.StringCleaners;

public class StringListNonValidWordCleanerTests
{
    private readonly StringListNonValidWordCleaner _cleaner;

    public StringListNonValidWordCleanerTests()
    {
        _cleaner = new();
        _cleaner.Construct(["BannedWord", "ForbiddenWord",]);
    }

    [Fact]
    public void Clean_ShouldRemoveBannedWords()
    {
        //Arrange
        var input = new List<string> { "some", "word", "BannedWord", "GreatWord", "ForbiddenWord", };

        //Act
        var result = _cleaner.Clean(input);

        //Assert
        result.Should().Contain(["word", "GreatWord",]);
    }

    [Fact]
    public void Clean_ShouldRemoveEmptyOrWhiteSpaceWords()
    {
        //Arrange
        var input = new List<string> { null!, "", "     ", "", "                     ", "hello", "world", };

        //Act
        var result = _cleaner.Clean(input);

        //Assert
        result.Should().Contain(["hello", "world",]);
    }

    [Fact]
    public void Clean_ShouldRemoveShortWords()
    {
        //Arrange
        var input = new List<string> { "a", "is", "no", "yo", "la", "big", "wow", };

        //Act
        var result = _cleaner.Clean(input);

        //Assert
        result.Should().Contain(["big", "wow",]);
    }

    [Fact]
    public void Clean_ShouldReturnEmpty_WhenAllWordsAreInvalid()
    {
        //Arrange
        var input = new List<string> { "a", "is", "no", "yo", "la", null!, "", "   ", "         ", "BannedWord", "ForbiddenWord", "you", "your", };

        //Act
        var result = _cleaner.Clean(input);

        //Assert
        result.Should().BeEmpty();
    }
}
