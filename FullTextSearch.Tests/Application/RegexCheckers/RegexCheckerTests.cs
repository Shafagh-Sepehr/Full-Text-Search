using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.RegexCheckers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.RegexCheckers;

public class RegexCheckerTests
{
    private readonly RegexChecker             _regexChecker;
    private readonly IEmailRegexChecker       _emailRegexChecker;
    private readonly IPhoneNumberRegexChecker _phoneNumberRegexChecker;
    private readonly IUrlRegexChecker         _urlRegexChecker;

    private readonly string _trueInput;
    private readonly string _falseInput;

    public RegexCheckerTests()
    {
        _emailRegexChecker = Substitute.For<IEmailRegexChecker>();
        _phoneNumberRegexChecker = Substitute.For<IPhoneNumberRegexChecker>();
        _urlRegexChecker = Substitute.For<IUrlRegexChecker>();
        _regexChecker = new(_emailRegexChecker, _phoneNumberRegexChecker, _urlRegexChecker);

        _trueInput = " original true input ";
        _falseInput = " original false input ";
    }

    [Fact]
    public void HasEmail_WhenCorrectlyCalled_ShouldCallMatchesMethodOnEmailRegexChecker()
    {
        // Arrange
        _emailRegexChecker.Matches(_trueInput).Returns(true);
        _emailRegexChecker.Matches(_falseInput).Returns(false);
        
        // Act
        var trueResult = _regexChecker.HasEmail(_trueInput);
        var falseResult = _regexChecker.HasEmail(_falseInput);
        
        // Assert
        trueResult.Should().Be(true);
        falseResult.Should().Be(false);
        _emailRegexChecker.Received(1).Matches(_trueInput);
        _emailRegexChecker.Received(1).Matches(_falseInput);
    }
    
    [Fact]
    public void HasPhoneNumber_WhenCorrectlyCalled_ShouldCallMatchesMethodOnPhoneNumberRegexChecker()
    {
        // Arrange
        _phoneNumberRegexChecker.Matches(_trueInput).Returns(true);
        _phoneNumberRegexChecker.Matches(_falseInput).Returns(false);
        
        // Act
        var trueResult = _regexChecker.HasPhoneNumber(_trueInput);
        var falseResult = _regexChecker.HasPhoneNumber(_falseInput);
        
        // Assert
        trueResult.Should().Be(true);
        falseResult.Should().Be(false);
        _phoneNumberRegexChecker.Received(1).Matches(_trueInput);
        _phoneNumberRegexChecker.Received(1).Matches(_falseInput);
    }
    
    
    [Fact]
    public void HasUrl_WhenCorrectlyCalled_ShouldCallMatchesMethodOnUrlRegexChecker()
    {
        // Arrange
        _urlRegexChecker.Matches(_trueInput).Returns(true);
        _urlRegexChecker.Matches(_falseInput).Returns(false);
        
        // Act
        var trueResult = _regexChecker.HasUrl(_trueInput);
        var falseResult = _regexChecker.HasUrl(_falseInput);
        
        // Assert
        trueResult.Should().Be(true);
        falseResult.Should().Be(false);
        _urlRegexChecker.Received(1).Matches(_trueInput);
        _urlRegexChecker.Received(1).Matches(_falseInput);
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var emailRegexChecker = Substitute.For<IEmailRegexChecker>();
        var phoneNumberRegexChecker = Substitute.For<IPhoneNumberRegexChecker>();
        var urlRegexChecker = Substitute.For<IUrlRegexChecker>();

        // Act
        Action act1 = () => new RegexChecker(null!, phoneNumberRegexChecker, urlRegexChecker);
        Action act2 = () => new RegexChecker(emailRegexChecker, null!, urlRegexChecker);
        Action act3 = () => new RegexChecker(emailRegexChecker, phoneNumberRegexChecker, null!);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
