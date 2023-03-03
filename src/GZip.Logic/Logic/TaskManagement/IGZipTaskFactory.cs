using GZip.Logic.Logic.Operations;
using GZip.Logic.Models;

namespace GZip.Logic.Logic.TaskManagement;

/// <summary> Creation of tasks objects </summary>
public interface IGZipTaskFactory
{
    /// <summary> Creation of task object </summary>
    /// <returns> Created object </returns>
    GZipTask Create(FilePaths pathParams, TaskSynchronizationParams syncParams,
        FileChunk chunk, IOperation operation);
}