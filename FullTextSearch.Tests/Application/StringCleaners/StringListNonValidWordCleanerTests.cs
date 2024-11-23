using FluentAssertions;
using FullTextSearch.Application.StringCleaners.StringListNonValidWordCleaner.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.StringCleaners;

public class StringListNonValidWordCleanerTests
{
    private readonly StringListNonValidWordCleaner _cleaner;
    private readonly IAppSettings                  _appSettings;
    
    public StringListNonValidWordCleanerTests()
    {
        _appSettings = Substitute.For<IAppSettings>();
        _appSettings.bannedWords.Returns(["BannedWord", "ForbiddenWord",]);
        _cleaner = new(_appSettings);
    }
    
    [Fact]
    public void Clean_WhenCorrectlyCalled_ShouldRemoveBannedWords()
    {
        // Arrange
        var input = new List<string> { "some", "word", "BannedWord", "GreatWord", "ForbiddenWord", };
        
        // Act
        var result = _cleaner.Clean(input);
        
        // Assert
        result.Should().Contain(["word", "GreatWord",]);
    }
    
    [Fact]
    public void Clean_WhenHasEmptyOrWhiteSpaceWords_ShouldRemoveEmptyOrWhiteSpaceWords()
    {
        // Arrange
        var input = new List<string> { null!, "", "     ", "", "                     ", "hello", "world", };
        
        // Act
        var result = _cleaner.Clean(input);
        
        // Assert
        result.Should().Contain(["hello", "world",]);
    }
    
    [Fact]
    public void Clean_WhenHasShortWords_ShouldRemoveShortWords()
    {
        // Arrange
        var input = new List<string> { "a", "is", "no", "yo", "la", "big", "wow", };
        
        // Act
        var result = _cleaner.Clean(input);
        
        // Assert
        result.Should().Contain(["big", "wow",]);
    }
    
    [Fact]
    public void Clean_WhenAllWordsAreInvalid_ShouldReturnEmpty()
    {
        // Arrange
        var input = new List<string> { "a", "is", "no", "yo", "la", null!, "", "   ", "         ", "BannedWord", "ForbiddenWord", "you", "your", };
        
        // Act
        var result = _cleaner.Clean(input);
        
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IAppSettings appSettings = null!;

        // Act
        Action act = () => new StringListNonValidWordCleaner(appSettings);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
