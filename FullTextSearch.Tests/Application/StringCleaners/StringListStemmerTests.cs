using FluentAssertions;
using FullTextSearch.Application.StringCleaners.StringListStemmer.Services;
using NSubstitute;
using Porter2Stemmer;

namespace FullTextSearch.Tests.Application.StringCleaners;

public class StringListStemmerTests
{
    private readonly IPorter2Stemmer   _stemmer;
    private          StringListStemmer _stringListStemmer;
    
    public StringListStemmerTests()
    {
        _stemmer = Substitute.For<IPorter2Stemmer>();
        _stringListStemmer = new StringListStemmer(_stemmer);
    }
    
    [Fact]
    public void Stem_ShouldReturnStemmedWords()
    {
        //Arrange
        var input = new List<string> { "running", "jumps", "easily" };
        _stemmer.Stem("running").Returns(new StemmedWord("run","running"));
        _stemmer.Stem("jumps").Returns(new StemmedWord("jump","jumps"));
        _stemmer.Stem("easily").Returns(new StemmedWord("easy","easily"));
        
        //Act
        var result = _stringListStemmer.Stem(input);

        //Assert
        result.Should().BeEquivalentTo(["run","jump","easy",]);
    }
    
    [Fact]
    public void Stem_ShouldHandleEmptyInput()
    {
        //Arrange
        var input = new List<string>();
        
        //Act
        var result = _stringListStemmer.Stem(input);

        //Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Arrange
        IPorter2Stemmer stemmer = null!;

        // Act & Assert
        Action act = () => new StringListStemmer(stemmer);
        act.Should().Throw<ArgumentNullException>();
    }
}
