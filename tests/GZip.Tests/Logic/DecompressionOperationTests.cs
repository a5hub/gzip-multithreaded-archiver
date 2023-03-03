using FileProcessor.Configuration;
using FileProcessor.Logic.Archivation;
using FileProcessor.Logic.Operations;
using Moq;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Xunit;

namespace FileProcessor.Test.Logic
{
    public class DecompressionOperationTests
    {
        private readonly DecompressionOperation target;
        private readonly Mock<IAppConfig> appConfig = new Mock<IAppConfig>();
        private readonly Mock<IArchiveProvider> archiveProvider = new Mock<IArchiveProvider>();
        private readonly MockFileSystem mockFileSystem;

        public DecompressionOperationTests()
        {
            mockFileSystem = new MockFileSystem();
            target = new DecompressionOperation(appConfig.Object, archiveProvider.Object, mockFileSystem);
        }

        [Fact]
        public void SplitFileTest_WorksFine()
        {
            // arrange
            var filePath = @"C:\temp\in.txt";

            byte[] fileBytesData =
            {
                0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00,
                1, 2, 3, 4, 5,
                0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00,
                5, 4, 3, 2, 1
            };

            var fileData = new MockFileData(fileBytesData);

            mockFileSystem.AddFile(filePath, fileData);
            var chunkSize = 20;

            // expectations
            appConfig.SetupGet(_ => _.CompressionChunkSize).Returns(chunkSize);

            // act
            var expected = target.SplitFile(filePath).ToList();

            // assert
            Assert.Equal(2, expected.Count);
            appConfig.VerifyAll();
        }

        [Fact]
        public void ProcessChunkTest_WorksFine()
        {
            // arrange
            var input = new byte[] { 1, 2, 3, 4, 5 };
            var expected = new byte[] { 5, 4, 3, 2, 1 };

            // expectations
            archiveProvider.Setup(_ => _.Decompress(input)).Returns(expected);

            // act
            var actual = target.ProcessChunk(input);

            // assert
            Assert.Equal(expected, actual);
            archiveProvider.VerifyAll();
        }
    }
}
