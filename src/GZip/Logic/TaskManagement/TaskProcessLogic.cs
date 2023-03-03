using FileProcessor.Common;
using FileProcessor.Logic.FileAccess;
using Serilog;

namespace FileProcessor.Logic.TaskManagement
{
    /// <summary> Generic task process logic for both compression and decompression parts </summary>
    public class TaskProcessLogic : ITaskProcessLogic
    {
        /// <summary> Manage access to physical files </summary>
        private readonly IFileAccessor _fileManager;

        /// <summary> Generic task process logic for both compression and decompression parts </summary>
        /// <param name="fileManager"> Manage access to physical files </param>
        public TaskProcessLogic(IFileAccessor fileManager)
        {
            _fileManager = fileManager;
        }

        /// <summary> Process one task </summary>
        /// <param name="task"> Compression or decompression gzip task </param>
        public void ProcessTask(GZipTask task)
        {
            var fileChunk = _fileManager.ReadFileChunk(task.FilePathParams.InputFilePath,
                task.FileChunk.StartPosition, task.FileChunk.ReadBytes);

            var chunk = task.Operation.ProcessChunk(fileChunk);

            // After parallel execution of read and process operation
            // executed sequential write to file operation
            WriteToFile(task, chunk);
        }

        /// <summary> Sequential method to write data chunk to file </summary>
        /// <param name="task"> Task to process </param>
        /// <param name="chunk"> Data chunk to save to file </param>
        private void WriteToFile(GZipTask task, byte[] chunk)
        {
            Guard.NotNull(task, $"{nameof(task)}");
            Guard.NotNull(chunk, $"{nameof(chunk)}");

            var canProceedCallbackFunc = task.TaskSynchronizationParams.CanProceedFunc;
            var taskNumber = task.TaskSynchronizationParams.TaskNumber;
            var resetEvent = task.TaskSynchronizationParams.ResetEvent;

            while (true)
            {
                var taskCanProceedExecution = canProceedCallbackFunc(taskNumber);
                if (!taskCanProceedExecution)
                {
                    resetEvent.Wait();
                }

                taskCanProceedExecution = canProceedCallbackFunc(taskNumber);
                if (taskCanProceedExecution)
                {
                    resetEvent.Reset();
                    _fileManager.WriteFileChunk(task.FilePathParams.OutputFilePath, chunk);

                    Log.Information($"Task {task.TaskSynchronizationParams.TaskNumber} executed");

                    break;
                }
            }
        }
    }
}
