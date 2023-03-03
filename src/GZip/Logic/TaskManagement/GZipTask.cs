using FileProcessor.Common;
using FileProcessor.Logic.Operations;
using FileProcessor.Models;

namespace FileProcessor.Logic.TaskManagement
{
    /// <summary> GZip task logic implementation </summary>
    public class GZipTask
    {
        /// <summary> Input and output file paths </summary>
        public FilePaths FilePathParams { get; }

        /// <summary> Parameters needed to organize thread task synchronization </summary>
        public TaskSynchronizationParams TaskSynchronizationParams { get; }

        /// <summary> Contains parameters needed during compression logic </summary>
        public FileChunk FileChunk { get; }
        
        /// <summary> Process operation type </summary>
        public IOperation Operation { get; }

        /// <summary> Compression task logic implementation </summary>
        /// <param name="filePaths"> Input and output file paths </param>
        /// <param name="taskSynchronizationParams"> Parameters needed to organize thread task synchronization </param>
        /// <param name="fileChunk"> Contains parameters needed during compression logic </param>
        /// <param name="operation"> Process operation type </param>
        public GZipTask(FilePaths filePaths,
            TaskSynchronizationParams taskSynchronizationParams,
            FileChunk fileChunk,
            IOperation operation)
        {
            Guard.NotNull(filePaths, $"{nameof(filePaths)}");
            Guard.NotNull(taskSynchronizationParams, $"{nameof(taskSynchronizationParams)}");
            Guard.NotNull(fileChunk, $"{nameof(fileChunk)}");
            Guard.NotNull(operation, $"{nameof(operation)}");

            FilePathParams = filePaths;
            TaskSynchronizationParams = taskSynchronizationParams;
            FileChunk = fileChunk;
            Operation = operation;
        }
    }
}
