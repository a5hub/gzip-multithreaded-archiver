using System.IO.Abstractions.TestingHelpers;
using System.Text;
using GZip.Logic.Logic.FileAccess;
using Xunit;

namespace GZip.Tests.Logic;

public class FileAccessorTests
{
    private readonly FileAccessor target;
    private readonly MockFileSystem mockFileSystem;

    public FileAccessorTests()
    {
        mockFileSystem = new MockFileSystem();
        target = new FileAccessor(mockFileSystem);
    }

    [Fact]
    public void ReadFileChunkTest_WorksFine()
    {
        // arrange
        var filePath = @"C:\temp\in.txt";
        var fileData = new MockFileData("test data");
        var startPosition = 5L;
        var chunkLength = 4;
            
        mockFileSystem.AddFile(filePath, fileData);

        // act
        var actualBytes = target.ReadFileChunk(filePath, startPosition, chunkLength);
        var actual = Encoding.ASCII.GetString(actualBytes);

        // assert
        Assert.Equal("data", actual);
    }

    [Fact]
    public void WriteFileChunkTest_WorksFine()
    {
        // arrange
        var filePath = @"C:\temp\in.txt";
        var fileData = new MockFileData("test data");
        var fileBytesData = Encoding.ASCII.GetBytes(fileData.TextContents);
            
        mockFileSystem.AddFile(filePath, null);

        // act
        target.WriteFileChunk(filePath, fileBytesData);

        var mockOutputFile = mockFileSystem.GetFile(filePath);
        var actual = mockOutputFile.TextContents;

        // assert
        Assert.Equal(fileData.TextContents, actual);
    }
}