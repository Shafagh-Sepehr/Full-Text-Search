using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Abstractions;
using FullTextSearch.Application.RegexCheckers.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.RegexCheckers;

public class RegexCheckerTests
{
    private readonly IEmailRegexChecker       _emailRegexChecker;
    private readonly IPhoneNumberRegexChecker _phoneNumberRegexChecker;
    private readonly IUrlRegexChecker         _urlRegexChecker;
    private readonly RegexChecker             _regexChecker;

    private readonly string _originalTrueInput;
    private readonly string _originalFalseInput;

    public RegexCheckerTests()
    {
        _emailRegexChecker = Substitute.For<IEmailRegexChecker>();
        _phoneNumberRegexChecker = Substitute.For<IPhoneNumberRegexChecker>();
        _urlRegexChecker = Substitute.For<IUrlRegexChecker>();
        _regexChecker = new(_emailRegexChecker, _phoneNumberRegexChecker,_urlRegexChecker);

        _originalTrueInput = " original true input ";
        _originalFalseInput = " original false input ";
    }

    [Fact]
    public void HasEmail_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var trueInput = new string(_originalTrueInput);
        var falseInput = new string(_originalFalseInput);
        const bool expectedResultTrue = true;
        const bool expectedResultFalse = false;
        
        _emailRegexChecker.Matches(trueInput).Returns(expectedResultTrue);
        _emailRegexChecker.Matches(falseInput).Returns(expectedResultFalse);
        
        // Act
        var trueResult = _regexChecker.HasEmail(trueInput);
        var falseResult = _regexChecker.HasEmail(falseInput);
        
        // Assert
        trueResult.Should().Be(expectedResultTrue);
        falseResult.Should().Be(expectedResultFalse);
        _emailRegexChecker.Received(1).Matches(trueInput); 
        _emailRegexChecker.Received(1).Matches(falseInput); 
        
        // Verify that the original inputs are unchanged
        trueInput.Should().BeEquivalentTo(_originalTrueInput);
        falseInput.Should().BeEquivalentTo(_originalFalseInput);
    }
    
    [Fact]
    public void GetNotDocuments_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var trueInput = new string(_originalTrueInput);
        var falseInput = new string(_originalFalseInput);
        const bool expectedResultTrue = true;
        const bool expectedResultFalse = false;
        
        _phoneNumberRegexChecker.Matches(trueInput).Returns(expectedResultTrue);
        _phoneNumberRegexChecker.Matches(falseInput).Returns(expectedResultFalse);
        
        // Act
        var trueResult = _regexChecker.HasPhoneNumber(trueInput);
        var falseResult = _regexChecker.HasPhoneNumber(falseInput);
        
        // Assert
        trueResult.Should().Be(expectedResultTrue);
        falseResult.Should().Be(expectedResultFalse);
        _phoneNumberRegexChecker.Received(1).Matches(trueInput); 
        _phoneNumberRegexChecker.Received(1).Matches(falseInput); 
        
        // Verify that the original inputs are unchanged
        trueInput.Should().BeEquivalentTo(_originalTrueInput);
        falseInput.Should().BeEquivalentTo(_originalFalseInput);
    }
    
    
    [Fact]
    public void HasUrl_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var trueInput = new string(_originalTrueInput);
        var falseInput = new string(_originalFalseInput);
        const bool expectedResultTrue = true;
        const bool expectedResultFalse = false;
        
        _urlRegexChecker.Matches(trueInput).Returns(expectedResultTrue);
        _urlRegexChecker.Matches(falseInput).Returns(expectedResultFalse);
        
        // Act
        var trueResult = _regexChecker.HasUrl(trueInput);
        var falseResult = _regexChecker.HasUrl(falseInput);
        
        // Assert
        trueResult.Should().Be(expectedResultTrue);
        falseResult.Should().Be(expectedResultFalse);
        _urlRegexChecker.Received(1).Matches(trueInput); 
        _urlRegexChecker.Received(1).Matches(falseInput); 
        
        // Verify that the original inputs are unchanged
        trueInput.Should().BeEquivalentTo(_originalTrueInput);
        falseInput.Should().BeEquivalentTo(_originalFalseInput);
    }
    
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IEmailRegexChecker       emailRegexChecker = null!;
        IPhoneNumberRegexChecker phoneNumberRegexChecker = null!;
        IUrlRegexChecker         urlRegexChecker = null!;

        // Act & Assert
        Action act = () => new RegexChecker(emailRegexChecker,phoneNumberRegexChecker, urlRegexChecker);
        act.Should().Throw<ArgumentNullException>();
    }
}
