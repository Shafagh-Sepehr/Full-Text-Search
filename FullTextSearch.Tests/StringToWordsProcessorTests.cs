using CodeStar2;
using FluentAssertions;
using NSubstitute;
using Porter2Stemmer;

namespace FullTextSearch.Tests;

public class StringToWordsProcessorTests
{
    private readonly IPorter2Stemmer _stemmer = Substitute.For<IPorter2Stemmer>();
    
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void TestDifferentStrings(string text,string[] expectedResult, IEnumerable<string>? banned)
    {
        //Arrange
        _stemmer.Stem(Arg.Any<string>()).Returns(callInfo =>
        {
            var input = callInfo.Arg<string>();
            return new StemmedWord(input, input);
        });
        var wordsProcessor = new StringToWordsProcessor(banned);
        

        //Act
        IEnumerable<string> words = wordsProcessor.TrimSplitAndStemString(text, _stemmer);

        //Assert
        words.Should().BeEquivalentTo(expectedResult);
    }
    
    
    
    
    public static IEnumerable<object?[]> TestData()
    {
        yield return ["hello world", new[] { "hello",}, new[] { "world" },];
        yield return ["hello world", new[] { "hello", "world"}, null,];
        yield return ["hello                  world", new[] { "hello", "world"}, null,];
        yield return ["hello     !!_#@&(#$%^%!_#       world", new[] { "hello", "world"}, null,];
        yield return ["       hello     !!_#@&(#$%^%!_#       world       ", new[] { "hello", "world"}, null,];
        yield return ["hello email@gmail.co 09360968954 +989879876525 https://linktowebsite.com http://thisishttpsite.com:80/yooo/subquery", new[] { "hello"}, null,];
        yield return ["once upon a time", new[] { "upon","time"}, null,];
        yield return ["+!@(#)(!(&#^%$upon\\\"'|/`~!#$", new[] { "upon"}, null,];
        yield return ["123", new[] { "123"}, null,];
        yield return ["1.3", new[] { "1.3",}, null,];
        yield return ["1231654d", Array.Empty<object>(), null,];
        yield return ["12", Array.Empty<object>(), null,];
        yield return ["we er we w q a vb gg", Array.Empty<object>(), null,];
        yield return ["once-upon-a-time", new[] { "upon","time"}, null,];
        yield return ["once/upon/a/time", new[] { "upon","time"}, null,];
    }
}
