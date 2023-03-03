using GZip.Logic.Logic.Operations;
using GZip.Logic.Logic.Threading;
using GZip.Logic.Models;

namespace GZip.Logic.Logic.TaskManagement;

/// <summary> Tasks creating and executing logic </summary>
public interface ITaskManager : IThreadExecutable
{
    /// <summary> Fill queue with tasks </summary>
    /// <param name="pathParams"> Paths for input and output files </param>
    /// <param name="operation"> Process operation type </param>
    void CreateTasks(FilePaths pathParams, IOperation operation);
}