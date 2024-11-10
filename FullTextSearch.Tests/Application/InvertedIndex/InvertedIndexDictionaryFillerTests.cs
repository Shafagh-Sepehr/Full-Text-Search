using FluentAssertions;
using FullTextSearch.Application.InvertedIndex.Abstractions;
using FullTextSearch.Application.InvertedIndex.Services;
using NSubstitute;

namespace FullTextSearch.Tests.Application.InvertedIndex;

public class InvertedIndexDictionaryFillerTests
{
    private readonly InvertedIndexDictionaryFiller _filler;
    private readonly IStringToWordsProcessor       _stringToWordsProcessor;
    
    public InvertedIndexDictionaryFillerTests()
    {
        _stringToWordsProcessor = Substitute.For<IStringToWordsProcessor>();
        _filler = new(_stringToWordsProcessor);
    }
    
    [Fact]
    public void Build_WhenCorrectlyCalled_ShouldReturnCorrectInvertedIndex()
    {
        // Arrange
        const string text1 = "first file contents";
        const string text2 = "second file contents";
        const string text3 = "third file content are not second";
        var processed1 = new List<string> { "first", "file", "contents", "wow", "another", };
        var processed2 = new List<string> { "second", "file", "contents", "another", "two", };
        var processed3 = new List<string> { "third", "file", "content", "are", "not", "second", "wow", };
        var expectedInvertedIndex = new Dictionary<string, List<string>>
        {
            { "first", ["file1",] },
            { "file", ["file2", "file3", "file1",] },
            { "contents", ["file2", "file1",] },
            { "content", ["file3",] },
            { "second", ["file2", "file3",] },
            { "third", ["file3",] },
            { "are", ["file3",] },
            { "not", ["file3",] },
            { "wow", ["file1", "file3",] },
            { "another", ["file1", "file2",] },
            { "two", ["file2",] },
        };
        
        const string folderPath = "/tmp/full-text-search-tests";
        const string path1 = $"{folderPath}/file1";
        const string path2 = $"{folderPath}/file2";
        const string path3 = $"{folderPath}/file3";
        
        Directory.CreateDirectory("/tmp/full-text-search-tests");
        File.WriteAllText(path1, text1);
        File.WriteAllText(path2, text2);
        File.WriteAllText(path3, text3);
        var lastAccessTime1 = File.GetLastAccessTime(path1);
        var lastAccessTime2 = File.GetLastAccessTime(path2);
        var lastAccessTime3 = File.GetLastAccessTime(path3);
        
        _stringToWordsProcessor.TrimSplitAndStemString(text1).Returns(processed1);
        _stringToWordsProcessor.TrimSplitAndStemString(text2).Returns(processed2);
        _stringToWordsProcessor.TrimSplitAndStemString(text3).Returns(processed3);
        
        // Act
        var result = _filler.Build(folderPath);
        
        // Assert
        result.Should().BeEquivalentTo(expectedInvertedIndex);
        
        _stringToWordsProcessor.Received(1).TrimSplitAndStemString(text1);
        _stringToWordsProcessor.Received(1).TrimSplitAndStemString(text2);
        _stringToWordsProcessor.Received(1).TrimSplitAndStemString(text3);
        lastAccessTime1.Should().BeBefore(File.GetLastAccessTime(path1));
        lastAccessTime2.Should().BeBefore(File.GetLastAccessTime(path2));
        lastAccessTime3.Should().BeBefore(File.GetLastAccessTime(path3));
    }
    
    [Fact]
    public void Build_WhenPathIsInvalid_ShouldThrowException()
    {
        // Arrange
        const string path = "/invaliiiiiiiiid/path/this/really/should/not/exist ?";
        
        // Act
        Action act = () => _filler.Build(path);
        
        // Assert
        act.Should().Throw<DirectoryNotFoundException>();
    }
    
    [Fact]
    public void Construct_WhenCorrectlyCalled_ShouldCallConstructOnStringToWordProcessor()
    {
        // Arrange
        IReadOnlyList<string> bannedWords = new List<string> { "val1", };
        
        // Act
        _filler.Construct(bannedWords);
        
        // Assert
        _stringToWordsProcessor.Received(1).Construct(bannedWords);
    }
    
    [Fact]
    public void Constructor_WhenADependencyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IStringToWordsProcessor stringToWordsProcessor = null!;
        
        // Act
        Action act = () => new InvertedIndexDictionaryFiller(stringToWordsProcessor);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
