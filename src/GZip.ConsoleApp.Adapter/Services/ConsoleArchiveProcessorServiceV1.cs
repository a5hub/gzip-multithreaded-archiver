using GZip.Common.Common;
using GZip.Logic.Logic.Operations;
using GZip.Logic.Logic.TaskManagement;
using GZip.Logic.Logic.Threading;
using GZip.Logic.Models;

namespace GZip.ConsoleApp.Adapter.Services;

public class ConsoleArchiveProcessorServiceV1 : IArchiveProcessorService
{
    /// <summary> Creates operation implementation dependent upon type </summary>
    private readonly IOperationFactory _operationFactory;
    
    /// <summary> Manages all threads in application </summary>
    private readonly IThreadManager _threadManager;

    /// <summary> Tasks creating and executing logic </summary>
    private readonly ITaskManager _taskManager;

    public ConsoleArchiveProcessorServiceV1(IOperationFactory operationFactory, 
        IThreadManager threadManager, 
        ITaskManager taskManager)
    {
        _operationFactory = operationFactory;
        _threadManager = threadManager;
        _taskManager = taskManager;
    }

    public async Task DoArchivation(FilePaths pathParams, OperationType operationType)
    {
        var operation = _operationFactory.Create(operationType);
        _taskManager.CreateTasks(pathParams, operation);
        _threadManager.ExecuteInParallel(_taskManager, UnexpectedTerminationProc);
    }
    
    /// <summary> Method which will be call in case of thread exception </summary>
    /// <param name="exception"> Internal thread exception </param>
    private void UnexpectedTerminationProc(Exception exception)
    {
        Guard.NotNull(exception, $"{nameof(exception)}");

        _threadManager.InterruptAllParallelExecutingThreads();
        Console.WriteLine(exception.Message);
        EnvironmentControl.Current.Exit(0);
    }
}