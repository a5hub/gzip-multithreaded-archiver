using System;
using System.Collections.Generic;
using GZip.Logic.Operations;
using GZip.Logic.TaskManagement;
using GZip.Models;
using Moq;
using Xunit;

namespace GZip.Tests.TaskManagement
{
    public class TaskManagerTests
    {
        private readonly TaskManager target;
        private readonly Mock<ITaskProcessLogic> taskProcessLogic = new Mock<ITaskProcessLogic>();
        private readonly Mock<ITaskSynchronizationParamsFactory> taskSyncParamsFactory
            = new Mock<ITaskSynchronizationParamsFactory>();
        private readonly Mock<IGZipTaskFactory> gzipTaskFactory = new Mock<IGZipTaskFactory>();
        private readonly Mock<ITaskQueue<GZipTask>> taskQueue = new Mock<ITaskQueue<GZipTask>>();

        public TaskManagerTests()
        {
            target = new TaskManager(taskProcessLogic.Object, taskSyncParamsFactory.Object,
                gzipTaskFactory.Object, taskQueue.Object);
        }

        [Fact]
        public void CreateTasksTest_EmptyChunks()
        {
            // arrange
            var inputFilePath = "path";
            var filePaths = new FilePaths
            {
                InputFilePath = inputFilePath
            };
            var operation = new Mock<IOperation>();

            var fileChunks = new List<FileChunk>();

            // expectations
            operation.Setup(_ => _.SplitFile(inputFilePath)).Returns(fileChunks);

            // act
            target.CreateTasks(filePaths, operation.Object);

            // assert
            operation.VerifyAll();
        }

        [Fact]
        public void CreateTasksTest_WorksFine()
        {
            // arrange
            var inputFilePath = "path";
            var filePaths = new FilePaths
            {
                InputFilePath = inputFilePath
            };
            var operation = new Mock<IOperation>();

            var chunk = new FileChunk {StartPosition = 0, ReadBytes = 10};
            var fileChunks = new List<FileChunk> { chunk };
            var taskNumber = 0;

            var syncParams = new TaskSynchronizationParams
            {
                TaskNumber = taskNumber,
                ResetEvent = target.ResetEvent,
                CanProceedFunc = target.CanProceed
            };

            var gZipTask = new GZipTask (filePaths,syncParams,chunk, operation.Object);

            // expectations
            operation.Setup(_ => _.SplitFile(inputFilePath)).Returns(fileChunks);
            taskSyncParamsFactory.Setup(_ => _.Create(taskNumber, target.ResetEvent, target.CanProceed)).Returns(syncParams);
            gzipTaskFactory.Setup(_ => _.Create(filePaths, syncParams, chunk, operation.Object)).Returns(gZipTask);
            taskQueue.Setup(_ => _.Enqueue(gZipTask));

            // act
            target.CreateTasks(filePaths, operation.Object);

            // assert
            operation.VerifyAll();
        }

        [Fact]
        public void ExecuteTest()
        {
            // arrange
            Action<Exception> exceptionHandler = (ex) => { };
            var filePath = new FilePaths();
            var syncParams = new TaskSynchronizationParams();
            var chunk = new FileChunk();
            var operation = new Mock<IOperation>();
            var gZipTask = new GZipTask(filePath, syncParams, chunk, operation.Object);

            // expectation
            taskQueue.SetupSequence(_ => _.NotEmpty())
                .Returns(true)
                .Returns(false);
            taskQueue.Setup(_ => _.TryDequeue(out gZipTask));
            taskProcessLogic.Setup(_ => _.ProcessTask(gZipTask));

            // act
            target.Execute(exceptionHandler);

            // assert
            taskProcessLogic.VerifyAll();
        }
    }
}
