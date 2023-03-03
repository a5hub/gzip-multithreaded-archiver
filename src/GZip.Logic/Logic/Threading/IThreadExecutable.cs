namespace GZip.Logic.Logic.Threading;

/// <summary> Interface for execution in new thread </summary>
public interface IThreadExecutable
{
    /// <summary> Method executed in parallel </summary>
    /// <param name="exceptionHandler"> Method which will be called in case of any thread raise an exception </param>
    void Execute(Action<Exception> exceptionHandler);
}