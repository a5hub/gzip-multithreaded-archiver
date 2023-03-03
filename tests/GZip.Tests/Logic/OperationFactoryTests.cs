using System;
using System.IO.Abstractions.TestingHelpers;
using GZip.Configuration;
using GZip.Logic.Archivation;
using GZip.Logic.Operations;
using Moq;
using Xunit;

namespace GZip.Tests.Logic
{
    public class OperationFactoryTests
    {
        private readonly OperationFactory target;
        private readonly Mock<IAppConfig> appConfig = new Mock<IAppConfig>();
        private readonly Mock<IArchiveProvider> archiveProvider = new Mock<IArchiveProvider>();
        private readonly MockFileSystem mockFileSystem;

        public OperationFactoryTests()
        {
            mockFileSystem = new MockFileSystem();
            target = new OperationFactory(appConfig.Object, archiveProvider.Object, mockFileSystem);
        }

        [Fact]
        public void CreateTest_WorksFine_CreatesCompressionOperation()
        {
            // arrange
            var operationType = OperationType.Compress;

            // act
            var actual = target.Create(operationType);

            // assert
            Assert.True(actual is CompressionOperation);
        }

        [Fact]
        public void CreateTest_WorksFine_CreatesDecompressionOperation()
        {
            // arrange
            var operationType = OperationType.Decompress;

            // act
            var actual = target.Create(operationType);

            // assert
            Assert.True(actual is DecompressionOperation);
        }

        [Fact]
        public void CreateTest_WorksFine_ThrowsUnexpectedOperationResult()
        {
            // arrange
            var operationType = OperationType.Default;

            // act
            try
            {
                target.Create(operationType);
                Assert.True(false, "Exception was not thrown!");
            }
            catch (Exception ex)
            {
                // assert
                Assert.True(ex is ArgumentException);
                Assert.Equal(Resources.UnexpectedOperationException, ex.Message);
            }
        }
    }
}
