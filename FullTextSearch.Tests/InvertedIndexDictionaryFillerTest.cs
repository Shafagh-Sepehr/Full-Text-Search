using System.Text.Json;
using FluentAssertions;
using FullTextSearch.Application.InvertedIndex;
using FullTextSearch.Application.InvertedIndex.Interfaces;
using NSubstitute;
using Porter2Stemmer;

namespace FullTextSearch.Tests;

public class InvertedIndexDictionaryFillerTest
{
    private readonly Dictionary<string, List<string>>? _expectedInvertedIndex =
        JsonSerializer.Deserialize<Dictionary<string, List<string>>>(InvertedIndexDictionaryBuilderTestData.InvertedIndexJson);
    
    private readonly IPorter2Stemmer         _stemmer                = Substitute.For<IPorter2Stemmer>();
    private readonly IStringToWordsProcessor _stringToWordsProcessor = Substitute.For<IStringToWordsProcessor>();


    [Fact]
    public void TestDifferentTexts()
    {
        //Arrange
        Directory.CreateDirectory("/tmp/unit_test_temp");
        foreach (TextTestPackage testPackage in InvertedIndexDictionaryBuilderTestData.TestData)
        {
            File.WriteAllText($"/tmp/unit_test_temp/{testPackage.FileName}", testPackage.Text);

            //mock IStringToWordProcessor
            _stringToWordsProcessor.TrimSplitAndStemString(testPackage.Text).Returns(testPackage.Words);
        }

        //mock IPorter2Stemmer
        _stemmer.Stem(Arg.Any<string>()).Returns(callInfo =>
        {
            var input = callInfo.Arg<string>();
            return new StemmedWord(input, input);
        });


        var invertedIndexDictionaryBuilder = new InvertedIndexDictionaryFiller(_stringToWordsProcessor);

        //Act
        Dictionary<string, List<string>> invertedIndex = invertedIndexDictionaryBuilder.Build("/tmp/unit_test_temp/");

        File.WriteAllText("/tmp/unit_test_temp/inv.json", JsonSerializer.Serialize(invertedIndex));

        //Assert
        invertedIndex.Should().BeEquivalentTo(_expectedInvertedIndex);
    }
}
