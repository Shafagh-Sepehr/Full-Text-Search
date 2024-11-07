using FluentAssertions;
using FullTextSearch.Application.DocumentsReader.Abstractions;
using FullTextSearch.Application.DocumentsReader.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.DocumentsReader;

public class DocumentReaderTests
{
    private readonly IAndDocumentsReader _andDocumentsReader;
    private readonly DocumentReader      _documentReader;
    private readonly INotDocumentsReader _notDocumentsReader;
    private readonly IOrDocumentsReader  _orDocumentsReader;

    private readonly Dictionary<string, List<string>> _originalInvertedIndex;
    private readonly List<string>                     _originalWords;
    private readonly HashSet<string>                  _originalExpectedDocuments;

    public DocumentReaderTests()
    {
        _andDocumentsReader = Substitute.For<IAndDocumentsReader>();
        _orDocumentsReader = Substitute.For<IOrDocumentsReader>();
        _notDocumentsReader = Substitute.For<INotDocumentsReader>();
        _documentReader = new(_andDocumentsReader, _orDocumentsReader, _notDocumentsReader);

        _originalInvertedIndex = new()
        {
            { "word1", ["doc1", "doc2",] },
            { "word2", ["doc3",] }
        };
        _originalWords = ["word1", "word2",];
        _originalExpectedDocuments = ["doc1", "doc2",];
    }

    [Fact]
    public void GetAndDocuments_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var andWords = new List<string>(_originalWords);
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        
        _andDocumentsReader.GetAndDocuments(invertedIndex, andWords).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetAndDocuments(invertedIndex, andWords);
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _andDocumentsReader.Received(1).GetAndDocuments(invertedIndex, andWords); 
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        andWords.Should().BeEquivalentTo(_originalWords);
        expectedDocuments.Should().BeEquivalentTo(_originalExpectedDocuments);
    }

    [Fact]
    public void GetOrDocuments_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var orWords = new List<string>(_originalWords);
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        
        _orDocumentsReader.GetOrDocuments(invertedIndex, orWords).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetOrDocuments(invertedIndex, orWords);
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _orDocumentsReader.Received(1).GetOrDocuments(invertedIndex, orWords);
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        orWords.Should().BeEquivalentTo(_originalWords);
        expectedDocuments.Should().BeEquivalentTo(_originalExpectedDocuments);
    }

    [Fact]
    public void GetNotDocuments_WhenCorrectlyCalled_ShouldNotModifyInputAndReturnValues()
    {
        // Arrange
        var invertedIndex = new Dictionary<string, List<string>>(_originalInvertedIndex);
        var notWords = new List<string>(_originalWords);
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        
        _notDocumentsReader.GetNotDocuments(invertedIndex, notWords).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetNotDocuments(invertedIndex, notWords);
        
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _notDocumentsReader.Received(1).GetNotDocuments(invertedIndex, notWords);
        
        // Verify that the original inputs are unchanged
        invertedIndex.Should().BeEquivalentTo(_originalInvertedIndex);
        notWords.Should().BeEquivalentTo(_originalWords);
        expectedDocuments.Should().BeEquivalentTo(_originalExpectedDocuments);
    }

    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var andReader = Substitute.For<IAndDocumentsReader>();
        var orReader = Substitute.For<IOrDocumentsReader>();
        var notReader = Substitute.For<INotDocumentsReader>();

        // Act
        Action act1 = () => new DocumentReader(null!, orReader, notReader);
        Action act2 = () => new DocumentReader(andReader, null!, notReader);
        Action act3 = () => new DocumentReader(andReader, orReader, null!);

        //Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
