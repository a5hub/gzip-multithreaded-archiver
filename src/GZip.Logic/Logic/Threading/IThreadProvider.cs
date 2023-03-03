namespace GZip.Logic.Logic.Threading;

/// <summary> Provides base thread functional </summary>
public interface IThreadProvider
{
    /// <summary> Create a new thread </summary>
    /// <param name="method"> Method which will be executed in separated thread </param>
    void CreateThread(Action method);

    /// <summary> Wait all started threads execution</summary>
    void WaitAllThreads();

    /// <summary> Start execution of all created threads </summary>
    void StartAllThreads();

    /// <summary> Aborts execution of all created threads </summary>
    void AbortAllThreads();
}