using System.Net;
using System.Text.Json;
using CodeStar2;
using CodeStar2.Interfaces;
using FluentAssertions;
using NSubstitute;
using Porter2Stemmer;
using Xunit.Abstractions;

namespace FullTextSearch.Tests;



public class InvertedIndexDictionaryBuilderTest
{
    

    private readonly IPorter2Stemmer         _stemmer               = Substitute.For<IPorter2Stemmer>();
    private readonly IStringToWordsProcessor _stringToWordsProcessor = Substitute.For<IStringToWordsProcessor>();

    
    private readonly Dictionary<string, List<string>>? _expectedInvertedIndex = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(InvertedIndexTestData.InvertedIndexJson);
    

    [Fact]
    public void TestDifferentTexts()
    {
        //Arrange
        Directory.CreateDirectory("/tmp/unit_test_temp");
        foreach (TextTestPackage testPackage in InvertedIndexTestData.TestData)
        {
            File.WriteAllText($"/tmp/unit_test_temp/{testPackage.FileName}", testPackage.Text);
            
            //mock IStringToWordProcessor
            _stringToWordsProcessor.TrimSplitAndStemString(testPackage.Text, _stemmer).Returns(testPackage.Words);
        }
        //mock IPorter2Stemmer
        _stemmer.Stem(Arg.Any<string>()).Returns(callInfo =>
        {
            var input = callInfo.Arg<string>();
            return new StemmedWord(input, input);
        });

        
        var invertedIndexDictionaryBuilder = new InvertedIndexDictionaryBuilder(_stringToWordsProcessor, _stemmer);

        //Act
        Dictionary<string, List<string>> invertedIndex = invertedIndexDictionaryBuilder.Build("/tmp/unit_test_temp/");
        
        File.WriteAllText("/tmp/unit_test_temp/inv.json",JsonSerializer.Serialize(invertedIndex));
        
        //Assert
        invertedIndex.Should().BeEquivalentTo(_expectedInvertedIndex);
    }
}
