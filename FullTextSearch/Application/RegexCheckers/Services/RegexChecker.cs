using FullTextSearch.Application.RegexCheckers.Abstractions;

namespace FullTextSearch.Application.RegexCheckers.Services;

internal class RegexChecker(IEmailRegexChecker emailChecker, IPhoneNumberRegexChecker phoneNumberChecker, IUrlRegexChecker urlChecker) : IRegexChecker
{
    private readonly IEmailRegexChecker       _emailChecker       = emailChecker ?? throw new ArgumentNullException(nameof(emailChecker));
    private readonly IPhoneNumberRegexChecker _phoneNumberChecker = phoneNumberChecker ?? throw new ArgumentNullException(nameof(phoneNumberChecker));
    private readonly IUrlRegexChecker         _urlChecker         = urlChecker ?? throw new ArgumentNullException(nameof(urlChecker));
    
    public bool HasEmail(string value) => _emailChecker.Matches(value);
    public bool HasPhoneNumber(string value) => _phoneNumberChecker.Matches(value);
    public bool HasUrl(string value) => _urlChecker.Matches(value);
    
}
