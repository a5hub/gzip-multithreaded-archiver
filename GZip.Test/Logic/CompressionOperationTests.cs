using FileProcessor.Configuration;
using FileProcessor.Logic.Archivation;
using FileProcessor.Logic.Operations;
using Moq;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Xunit;

namespace FileProcessor.Test.Logic
{
    public class CompressionOperationTests
    {
        private readonly CompressionOperation target;
        private readonly Mock<IAppConfig> appConfig = new Mock<IAppConfig>();
        private readonly Mock<IArchiveProvider> archiveProvider = new Mock<IArchiveProvider>();
        private readonly MockFileSystem mockFileSystem;

        public CompressionOperationTests()
        {
            mockFileSystem = new MockFileSystem();
            target = new CompressionOperation(appConfig.Object, archiveProvider.Object, mockFileSystem);
        }

        [Fact]
        public void SplitFileTest_WorksFine()
        {
            // arrange
            var filePath = @"C:\temp\in.txt";
            var fileData = new MockFileData("test data");
            mockFileSystem.AddFile(filePath, fileData);
            var chunkSize = 2;

            // expectations
            appConfig.SetupGet(_ => _.CompressionChunkSize).Returns(chunkSize);

            // act
            var expected = target.SplitFile(filePath).ToList();

            // assert
            Assert.Equal(5, expected.Count);
            appConfig.VerifyAll();
        }

        [Fact]
        public void ProcessChunkTest_WorksFine()
        {
            // arrange
            var input = new byte[] { 1, 2, 3, 4, 5 };
            var expected = new byte[] { 5, 4, 3, 2, 1 };

            // expectations
            archiveProvider.Setup(_ => _.Compress(input)).Returns(expected);

            // act
            var actual = target.ProcessChunk(input);

            // assert
            Assert.Equal(expected, actual);
            archiveProvider.VerifyAll();
        }
    }
}
