using GZip.Logic.Operations;
using GZip.Logic.Threading;
using GZip.Models;

namespace GZip.Logic.TaskManagement
{
    /// <summary> Tasks creating and executing logic </summary>
    public interface ITaskManager : IThreadExecutable
    {
        /// <summary> Fill queue with tasks </summary>
        /// <param name="pathParams"> Paths for input and output files </param>
        /// <param name="operation"> Process operation type </param>
        void CreateTasks(FilePaths pathParams, IOperation operation);
    }
}
