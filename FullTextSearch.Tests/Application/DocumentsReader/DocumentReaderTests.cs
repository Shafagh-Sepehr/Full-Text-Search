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

    private readonly IReadOnlyDictionary<string, List<string>> _invertedIndex;
    private readonly IReadOnlyList<string>                     _words;
    private readonly HashSet<string>                           _originalExpectedDocuments;

    public DocumentReaderTests()
    {
        _andDocumentsReader = Substitute.For<IAndDocumentsReader>();
        _orDocumentsReader = Substitute.For<IOrDocumentsReader>();
        _notDocumentsReader = Substitute.For<INotDocumentsReader>();
        _documentReader = new(_andDocumentsReader, _orDocumentsReader, _notDocumentsReader);

        _invertedIndex = new Dictionary<string, List<string>>
        {
            { "word1", ["doc1", "doc2",] },
            { "word2", ["doc3",] },
        };
        _words = ["word1", "word2",];
        _originalExpectedDocuments = ["doc1", "doc2",];
    }

    [Fact]
    public void GetAndDocuments_WhenCorrectlyCalled_ShouldNotModifyReturnValues()
    {
        // Arrange
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        _andDocumentsReader.GetAndDocuments(_invertedIndex, _words).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetAndDocuments(_invertedIndex, _words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _andDocumentsReader.Received(1).GetAndDocuments(_invertedIndex, _words);
        
        // Verify that the original values are unchanged
        expectedDocuments.Should().BeEquivalentTo(_originalExpectedDocuments);
    }

    [Fact]
    public void GetOrDocuments_WhenCorrectlyCalled_ShouldNotModifyReturnValues()
    {
        // Arrange
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        _orDocumentsReader.GetOrDocuments(_invertedIndex, _words).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetOrDocuments(_invertedIndex, _words);
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _orDocumentsReader.Received(1).GetOrDocuments(_invertedIndex, _words);
        
        // Verify that the original values are unchanged
        expectedDocuments.Should().BeEquivalentTo(_originalExpectedDocuments);
    }

    [Fact]
    public void GetNotDocuments_WhenCorrectlyCalled_ShouldNotModifyReturnValues()
    {
        // Arrange
        var expectedDocuments = new HashSet<string>(_originalExpectedDocuments);
        _notDocumentsReader.GetNotDocuments(_invertedIndex, _words).Returns(expectedDocuments);
        
        // Act
        var result = _documentReader.GetNotDocuments(_invertedIndex, _words);
        
        
        // Assert
        result.Should().BeEquivalentTo(expectedDocuments);
        _notDocumentsReader.Received(1).GetNotDocuments(_invertedIndex, _words);
        
        // Verify that the original values are unchanged
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

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }
}
