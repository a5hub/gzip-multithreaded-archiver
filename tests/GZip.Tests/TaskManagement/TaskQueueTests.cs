using GZip.Logic.Logic.Operations;
using GZip.Logic.Logic.TaskManagement;
using GZip.Logic.Models;
using Moq;
using Xunit;

namespace GZip.Tests.TaskManagement;

public class TaskQueueTests
{
    private TaskQueue target;

    public TaskQueueTests()
    {
        target = new TaskQueue();
    }

    [Fact]
    public void EnqueueTest_WorksFine()
    {
        // arrange
        var pathParams = new FilePaths();
        var syncParams = new TaskSynchronizationParams();
        var chunk = new FileChunk();
        var operation = new Mock<IOperation>();
        var task = new GZipTask(pathParams, syncParams, chunk, operation.Object);
        GZipTask actual;

        // act
        target.Enqueue(task);
        target.Queue.TryDequeue(out actual);

        // assert
        Assert.Equal(task, actual);
    }

    [Fact]
    public void TryDequeueTest_WorksFine()
    {
        // arrange
        var pathParams = new FilePaths();
        var syncParams = new TaskSynchronizationParams();
        var chunk = new FileChunk();
        var operation = new Mock<IOperation>();
        var task = new GZipTask(pathParams, syncParams, chunk, operation.Object);
        target.Queue.Enqueue(task);
        GZipTask actual;

        // act
        target.TryDequeue(out actual);

        // assert
        Assert.Equal(task, actual);
    }

    [Fact]
    public void NotEmpty_WorksFine_QueueEmpty()
    {
        // act
        var actual = target.NotEmpty();

        // assert
        Assert.False(actual);
    }

    [Fact]
    public void NotEmpty_WorksFine_QueueNotEmpty()
    {
        // arrange
        var pathParams = new FilePaths();
        var syncParams = new TaskSynchronizationParams();
        var chunk = new FileChunk();
        var operation = new Mock<IOperation>();
        var task = new GZipTask(pathParams, syncParams, chunk, operation.Object);
        target.Queue.Enqueue(task);

        // act
        var actual = target.NotEmpty();

        // assert
        Assert.True(actual);
    }
}