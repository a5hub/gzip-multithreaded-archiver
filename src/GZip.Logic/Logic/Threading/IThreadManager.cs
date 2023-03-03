namespace GZip.Logic.Logic.Threading;

/// <summary> Manages all threads in application </summary>
public interface IThreadManager
{
    /// <summary> Execute chosen method of object in parallel threads </summary>
    /// <param name="executedObj"> Object which method will be executed in several parallel threads </param>
    /// <param name="exceptionHandler"> Exception callback handler </param>
    void ExecuteInParallel(IThreadExecutable executedObj, 
        Action<Exception> exceptionHandler);

    /// <summary> Aborts execution of all created threads </summary>
    public void InterruptAllParallelExecutingThreads();
}