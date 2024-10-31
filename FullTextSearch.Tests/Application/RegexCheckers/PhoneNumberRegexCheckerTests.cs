using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Services;

namespace FullTextSearch.Tests.Application.RegexCheckers;

public class PhoneNumberRegexCheckerTests
{
    [Theory]
    [MemberData(nameof(TrueTestData))]
    public void Matches(string value, bool expectedResult)
    {
        //Arrange
        var phoneNumberRegexChecker = new PhoneNumberRegexChecker();

        //Act
        var result = phoneNumberRegexChecker.Matches(value);

        //Assert
        result.Should().Be(expectedResult);
    }

    public static IEnumerable<object?[]> TrueTestData()
    {
        yield return [" +1 234-567-8900", true,];
        yield return [" +44 20 1234 5678", true,];
        yield return [" +91 98765 43210", true,];
        yield return [" +49 30 12345678", true,];
        yield return ["+61 2 1234 5678", true,];
        yield return ["123-456-7890 ", true,];
        yield return ["123.456.7890 ", true,];
        yield return ["1234567890", true,];
        yield return ["123 456 7890", true,];
        yield return ["020 1234 5678", true,];
        yield return ["0121 123 4567 words", true,];
        yield return ["07700 900123", true,];
        yield return ["07890 123456", true,];
        yield return ["+33 1 23 45 67 89", true,];
        yield return ["+81 3-1234-5678", true,];
        yield return ["+61 4 1234 5678", true,];
        yield return ["+34 912 34 56 78", true,];
        yield return ["+39 06 12345678", true,];
        yield return ["123-456-7890 ext. 123", true,];
        yield return ["123.456.7890 #123", true,];
        yield return ["+1 234 567 8900", true,];
        yield return ["1234567890", true,];
    }
}
