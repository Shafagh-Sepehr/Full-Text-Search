using FluentAssertions;
using FullTextSearch.Application.StringCleaners.StringListCleaner.Services;

namespace FullTextSearch.Tests.Application.StringCleaners;

public class StringListCleanerTests
{
    private readonly StringListCleaner _stringListCleaner = new();

    [Theory]
    [MemberData(nameof(TestData))]
    public void Clean_ShouldReturnExpectedResults_WhenInputIsProvided(IEnumerable<string> input, IEnumerable<string> expectedResult)
    {
        // Act
        var result = _stringListCleaner.Clean(input).ToList();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return [new List<string> { ")(*&^%Hello!@#", "World123", "Test$%^&", }, new List<string> { "Hello", "World", "Test", },];
        yield return [new List<string>(), new List<string>(),];
        yield return [new List<string> { "123", "45", "6789", "1.2", }, new List<string> { "6789", "123", "", "1.2", },];
        yield return [new List<string> { " 123abc!@#", " 456def$%^&", "ghi789 ", }, new List<string> { "abc", "def", "ghi", },];
        yield return
        [
            new List<string> { " !*&^1234@#&", "this-words-are-special-char-seperated", "ghi789 ", },
            new List<string> { "this", "words", "are", "special", "char", "seperated", "1234", "ghi", },
        ];
    }
}
