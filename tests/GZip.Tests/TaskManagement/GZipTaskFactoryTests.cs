using FluentAssertions;
using GZip.Logic.Operations;
using GZip.Logic.TaskManagement;
using GZip.Models;
using Moq;
using Xunit;

namespace GZip.Tests.TaskManagement
{
    public class GZipTaskFactoryTests
    {
        private GZipTaskFactory target;

        public GZipTaskFactoryTests()
        {
            target = new GZipTaskFactory();
        }

        [Fact]
        public void CreateTest_WorksFine()
        {
            // arrange
            var pathParams = new FilePaths();
            var syncParams = new TaskSynchronizationParams();
            var chunk = new FileChunk();
            var operation = new Mock<IOperation>();
            var expected = new GZipTask(pathParams, syncParams, chunk, operation.Object);

            // act
            var actual = target.Create(pathParams, syncParams, chunk, operation.Object);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
