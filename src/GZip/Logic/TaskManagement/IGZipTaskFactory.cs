using FileProcessor.Logic.Operations;
using FileProcessor.Models;

namespace FileProcessor.Logic.TaskManagement
{
    /// <summary> Creation of tasks objects </summary>
    public interface IGZipTaskFactory
    {
        /// <summary> Creation of task object </summary>
        /// <returns> Created object </returns>
        GZipTask Create(FilePaths pathParams, TaskSynchronizationParams syncParams,
            FileChunk chunk, IOperation operation);
    }
}
