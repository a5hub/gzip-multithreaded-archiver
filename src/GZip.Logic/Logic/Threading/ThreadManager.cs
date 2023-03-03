using GZip.Common.Common;
using GZip.Logic.Configuration;

namespace GZip.Logic.Logic.Threading;

/// <summary> Manages all threads in application </summary>
public class ThreadManager : IThreadManager
{
    /// <summary> Provides base thread functional </summary>
    private readonly IThreadProvider _threadProvider;

    /// <summary> Application configuration  </summary>
    private readonly IAppConfig _appConfig;

    /// <summary> Manages all threads in application </summary>
    /// <param name="threadProvider"> Provides base thread functional </param>
    /// <param name="appConfig"> Application configuration </param>
    public ThreadManager(IThreadProvider threadProvider,
        IAppConfig appConfig)
    {
        _threadProvider = threadProvider;
        _appConfig = appConfig;
    }

    /// <summary> Execute chosen method of object in parallel threads </summary>
    /// <param name="executedObj"> Object which method will be executed in several parallel threads </param>
    /// <param name="exceptionHandler"> Exception callback handler </param>
    public void ExecuteInParallel(IThreadExecutable executedObj, 
        Action<Exception> exceptionHandler)
    {
        Guard.NotNull(executedObj, $"{nameof(executedObj)}");
        Guard.NotNull(exceptionHandler, $"{nameof(exceptionHandler)}");

        for (var i = 0; i < _appConfig.ProcessorsCount; i++)
        {
            _threadProvider.CreateThread(() => executedObj.Execute(exceptionHandler));
        }

        _threadProvider.StartAllThreads();
        _threadProvider.WaitAllThreads();
    }

    /// <summary> Aborts execution of all created threads </summary>
    public void InterruptAllParallelExecutingThreads()
    {
        _threadProvider.AbortAllThreads();
    }
}