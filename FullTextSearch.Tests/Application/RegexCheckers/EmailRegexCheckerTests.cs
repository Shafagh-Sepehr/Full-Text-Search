using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Services;

namespace FullTextSearch.Tests.Application.RegexCheckers;

public class EmailRegexCheckerTests
{
    //Arrange
    private readonly EmailRegexChecker _emailRegexChecker = new();
    
    [Theory]
    [MemberData(nameof(TrueTestData))]
    public void Matches(string value, bool expectedResult)
    {
        //Act
        var result = _emailRegexChecker.Matches(value);
        
        //Assert
        result.Should().Be(expectedResult);
    }

    public static IEnumerable<object?[]> TrueTestData()
    {
        yield return ["test.email@example.com", true,];
        yield return ["user.name+tag+sorting@example.com", true,];
        yield return ["user_name@example.co.uk", true,];
        yield return ["first.last@subdomain.example.com", true,];
        yield return ["simple@example.com", true,];
        yield return ["email@domain.com", true,];
        yield return ["user123@domain.org", true,];
        yield return ["name@domain.info", true,];
        yield return ["contact@company-name.com", true,];
        yield return ["user@123.456.789.000", true,];
        yield return ["name@domain.travel", true,];
        yield return ["example@domain.museum", true,];
        yield return ["test.email@domain.com", true,];
        yield return ["user@domain.cafe", true,];
        yield return ["name@domain.pro", true,];
        yield return ["user@domain.jobs", true,];
        yield return ["email@domain.xyz", true,];
        yield return ["name@domain.aero", true,];
        yield return ["user@domain.asia", true,];
        yield return ["plainaddress", false,];
        yield return ["@missingusername.com", false,];
        yield return ["username@.com", false,];
        yield return ["username@domain..com", false,];
        yield return ["username@domain,com", false,];
        yield return ["username@domain#example.com", false,];
        yield return ["user@domain..com", false,];
        yield return ["user@-domain.com", false,];
        yield return ["user@domain-.com", false,];
        yield return ["user@domain..com", false,];
        yield return ["user@.domain.com", false,];
        yield return ["user@domain..com", false,];
        yield return ["user@domain..co.uk", false,];
        yield return ["user@domain..com", false,];
    }
}
