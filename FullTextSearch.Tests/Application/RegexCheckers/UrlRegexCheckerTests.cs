using FluentAssertions;
using FullTextSearch.Application.RegexCheckers.Services;

namespace FullTextSearch.Tests.Application.RegexCheckers;

public class UrlRegexCheckerTests
{
    [Theory]
    [MemberData(nameof(TrueTestData))]
    public void Matches(string value, bool expectedResult)
    {
        //Arrange
        var urlRegexChecker = new UrlRegexChecker();

        //Act
        var result = urlRegexChecker.Matches(value);

        //Assert
        result.Should().Be(expectedResult);
    }

    public static IEnumerable<object?[]> TrueTestData()
    {
        yield return ["http://192.168.1.1:8080/home", true,];
        yield return ["https://www.example.com:443/products/item?id=123", true,];
        yield return ["ftp://ftp.example.com/files/document.pdf", true,];
        yield return ["http://10.0.0.5:3000/api/v1/users", true,];
        yield return ["https://subdomain.example.org/blog/post/2023/03", true,];
        yield return ["http://localhost:5000/dashboard", true,];
        yield return ["https://203.0.113.45:8443/secure/login", true,];
        yield return ["http://example.net:80/about", true,];
        yield return ["https://192.0.2.1:9000/services", true,];
        yield return ["ftp://files.example.com/uploads/image.jpg", true,];
        yield return ["http://example.com:1234/search?q=keyword", true,];
        yield return ["https://172.16.254.1:5001/checkout", true,];
        yield return ["http://mywebsite.com:3001/contact", true,];
        yield return ["https://198.51.100.2:8081/api/v2/products", true,];
        yield return ["http://localhost:4000/admin/settings", true,];
        yield return ["https://www.example.edu:443/courses/intro-to-programming", true,];
        yield return ["http://192.168.0.10:8080/status", true,];
        yield return ["ftp://ftp.example.org/public/README.txt", true,];
        yield return ["https://example.io:5000/portfolio", true,];
        yield return ["http://10.1.1.1:9001/feedback", true,];
        yield return ["http://10.1.1.1:9001/feedback", true,];
        yield return ["10.0.0.5:3000/api/v1/users", true,];
        yield return ["google.com", true,];
        yield return ["google.com/help/support", true,];
        yield return ["https://webauth.iut.ac.ir/cas/login?service=https%3A%2F%2Flogin.iut.ac.ir%2Flogin%2Fcas", true,];
        yield return ["https://dining.iut.ac.ir/#!/UserIndex", true,];
        yield return ["This is a simple text without a URL.",false];
        yield return ["Check out my email at example@domain.com!",false];
        yield return ["The price is $50.",false];
        yield return ["Just a random string with numbers 123456.",false];
        yield return ["This string has a hashtag #hashtag but no URL.",false];
        yield return ["My favorite number is 42.",false];
        yield return ["Visit us at our office in New York.",false];
        yield return ["This is a phone number: (123) 456-7890.",false];
        yield return ["Here is a list: apples, oranges, bananas.",false];
        yield return ["A quote: \",To be or not to be.\"",false];
        yield return ["This is a date: 2023-10-01.",false];
        yield return ["Some special characters: @#$%^&*()!",false];
        yield return ["A sentence with a period at the end.",false];
        yield return [@"This is a file path: C:\Users\Name\Documents\file.txt",false];
        yield return ["A mathematical expression: 2 + 2 = 4",false];
        yield return ["Random text with no URL: Lorem ipsum dolor sit amet.",false];
        yield return ["This is a command: git commit -m \",message\"",false];
        yield return ["A list of items: item1, item2, item3.",false];
        yield return ["A JSON object: {\",key\": \"value\"}",false];
        yield return ["This is a code snippet: for (int i = 0; i < 10; i++) {}",false];
    }
}
