using FileProcessor.Common;
using FileProcessor.Logic.Operations;
using FileProcessor.Logic.TaskManagement;
using FileProcessor.Logic.Threading;
using FileProcessor.Models;
using Moq;
using System;
using FluentAssertions;
using Xunit;

namespace FileProcessor.Test
{
    public class GZipProcessorTests
    {
        private readonly Mock<IThreadManager> threadManager= new Mock<IThreadManager>();
        private readonly Mock<ITaskManager> taskManager= new Mock<ITaskManager>();
        private readonly Mock<IOperationFactory> operationFactory= new Mock<IOperationFactory>();
        private readonly Mock<EnvironmentControl> environment = new Mock<EnvironmentControl>();

        private readonly GZipProcessor target;

        public GZipProcessorTests()
        {
            target = new GZipProcessor(threadManager.Object, taskManager.Object, operationFactory.Object);
        }

        [Fact]
        public void ProcessFileTest_WorksFine()
        {
            // arrange
            var pathParams = new FilePaths
            {
                InputFilePath = "input",
                OutputFilePath = "output"
            };
            var operationType = OperationType.Compress;
            var operation = new Mock<CompressionOperation>(null, null, null);

            // expectations
            operationFactory.Setup(_ => _.Create(operationType))
                .Returns(operation.Object);

            taskManager.Setup(_ => _.CreateTasks(pathParams, operation.Object))
                .Verifiable();

            threadManager.Setup(_ => _.ExecuteInParallel(taskManager.Object, It.IsAny<Action<Exception>>()));

            // act
            target.ProcessFile(pathParams, operationType);
            
            // assert
            operationFactory.VerifyAll();
            taskManager.VerifyAll();
            threadManager.VerifyAll();
        }

        [Fact]
        public void UnexpectedTerminationProcTest_WorksFine()
        {
            // arrange
            var exception = new Exception();
            EnvironmentControl.Current = environment.Object;

            // expectations
            threadManager.Setup(_ => _.InterruptAllParallelExecutingThreads());

            // act
            target.UnexpectedTerminationProc(exception);

            // assert
            threadManager.VerifyAll();
            environment.Verify(_ => _.Exit(0));
        }

        [Fact]
        public void RunTest_WorksFine()
        {
            // arrange
            var inputPath = @"c:\temp\filename.ext";
            var outputPath = @"c:\temp\filename.ext.gz";
            var args = new [] {"compress", inputPath, outputPath};
            var operationType = OperationType.Compress;
            var operation = new Mock<IOperation>();
            Action<Exception> action = (ex) => { };

            // expectations
            operationFactory.Setup(_ => _.Create(operationType)).Returns(operation.Object);
            taskManager.Setup(_ => _.CreateTasks(It.IsAny<FilePaths>(), operation.Object))
                .Callback<FilePaths, IOperation>((paths, operation) =>
                {
                    Assert.Equal(inputPath, paths.InputFilePath);
                    Assert.Equal(outputPath, paths.OutputFilePath);
                    operation.Should().BeEquivalentTo(operation);
                });

            threadManager.Setup(_ => _.ExecuteInParallel(It.IsAny<ITaskManager>(), It.IsAny<Action<Exception>>()))
                .Callback<IThreadExecutable, Action<Exception>>((taskManagerActual, actionActual) =>
                {
                    taskManagerActual.Should().BeEquivalentTo(taskManager.Object);
                });
            
            // act
            target.Run(args);

            // assert
            operationFactory.VerifyAll();
            taskManager.VerifyAll();
            threadManager.VerifyAll();
        }
    }
}
