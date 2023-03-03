using System;
using System.Threading;
using GZip.Logic.Logic.FileAccess;
using GZip.Logic.Logic.Operations;
using GZip.Logic.Logic.TaskManagement;
using GZip.Logic.Models;
using Moq;
using Xunit;

namespace GZip.Tests.TaskManagement;

public class TaskProcessLogicTests
{
    private readonly TaskProcessLogic target;
    private readonly Mock<IFileAccessor> fileManager = new Mock<IFileAccessor>();

    public TaskProcessLogicTests()
    {
        target = new TaskProcessLogic(fileManager.Object);
    }

    [Fact]
    public void ProcessTaskTest_WorksFine()
    {
        // arrange
        var inputFilePath = "inputPath";
        var outputFilePath = "outputPath";
        var filePaths = new FilePaths
        {
            InputFilePath = inputFilePath,
            OutputFilePath = outputFilePath
        };

        var taskNumber = 1;
        var resetEvent = new ManualResetEventSlim();
        Func<int, bool>  callback = number => true;
        var taskSyncParams = new TaskSynchronizationParams
        {
            TaskNumber = taskNumber,
            ResetEvent = resetEvent,
            CanProceedFunc = callback
        };
        var startPosition = 10;
        var readBytes = 200;
        var fileChunk = new FileChunk
        {
            StartPosition = startPosition,
            ReadBytes = readBytes 
        };
            
        var operation = new Mock<IOperation>();
        var gzipTask = new GZipTask(filePaths, taskSyncParams, fileChunk, operation.Object);

        var fileChunkRead = new byte[] {0, 100, 120, 210, 255};
        var fileChunkProcessed = new byte[] {10, 11, 12};

        // expectations
        fileManager.Setup(_ => _.ReadFileChunk(inputFilePath, startPosition, readBytes))
            .Returns(fileChunkRead);

        operation.Setup(_ => _.ProcessChunk(fileChunkRead))
            .Returns(fileChunkProcessed);

        fileManager.Setup(_ => _.WriteFileChunk(outputFilePath, fileChunkProcessed))
            .Verifiable();
                
        // act
        target.ProcessTask(gzipTask);

        // assert
        fileManager.VerifyAll();
    }
}