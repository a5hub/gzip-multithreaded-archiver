using System;
using System.Linq;
using System.Threading;
using GZip.Common;
using GZip.Logic.Operations;
using GZip.Models;
using Serilog;

namespace GZip.Logic.TaskManagement
{
    /// <summary> Tasks creating and executing logic </summary>
    public class TaskManager : ITaskManager
    {
        /// <summary> Thread shared event </summary>
        public ManualResetEventSlim ResetEvent { get; } = new ManualResetEventSlim(false);

        /// <summary> Showed last processed task </summary>
        public int TaskSequenceNumber { get; private set; }

        /// <summary> Generic task process logic for both compression and decompression parts </summary>
        private readonly ITaskProcessLogic _taskProcessLogic;

        /// <summary> Creation of task synchronization params objects </summary>
        private readonly ITaskSynchronizationParamsFactory _taskSyncParamsFactory;

        /// <summary> Creation of tasks params objects </summary>
        private readonly IGZipTaskFactory _gzipTaskFactory;

        /// <summary> Working queue for tasks to be processed </summary>
        private readonly ITaskQueue<GZipTask> _taskQueue;

        /// <summary> Tasks creating and executing logic </summary>
        /// <param name="taskProcessLogic"> Generic task process logic for both compression and decompression parts </param>
        /// <param name="taskSyncParamsFactory"> Creation of task synchronization params objects </param>
        /// <param name="gzipTaskFactory"> Creation of tasks params objects </param>
        /// <param name="taskQueue"> Working queue for tasks to be processed </param>
        public TaskManager(ITaskProcessLogic taskProcessLogic,
            ITaskSynchronizationParamsFactory taskSyncParamsFactory,
            IGZipTaskFactory gzipTaskFactory,
            ITaskQueue<GZipTask> taskQueue)
        {
            _taskProcessLogic = taskProcessLogic;
            _taskSyncParamsFactory = taskSyncParamsFactory;
            _gzipTaskFactory = gzipTaskFactory;
            _taskQueue = taskQueue;
        }

        /// <summary> Method executed in parallel </summary>
        /// <param name="exceptionHandler"> Method which will be called in case of any thread raise an exception </param>
        public void Execute(Action<Exception> exceptionHandler)
        {
            Guard.NotNull(exceptionHandler, $"{nameof(exceptionHandler)}");

            try
            {
                while (_taskQueue.NotEmpty())
                {
                    _taskQueue.TryDequeue(out var task);
                    if(task == null) return;
                    _taskProcessLogic.ProcessTask(task);
                    TaskSequenceNumber++;
                    ResetEvent.Set();
                }
            }
            catch (ThreadInterruptedException ex)
            {
                Log.Information($"Exception thrown {ex}");
            }
            catch (Exception ex)
            {
                Log.Information($"Exception thrown {ex}");
                exceptionHandler?.Invoke(ex);
            }
        }

        /// <summary> Callback method for check: Can task proceed work if sequential processing needed </summary>
        /// <param name="currentTaskNumber"> Number of processing task </param>
        /// <returns> True if task number is last in sequence </returns>
        public bool CanProceed(int currentTaskNumber) => TaskSequenceNumber == currentTaskNumber;

        /// <summary> Fill queue with tasks </summary>
        /// <param name="pathParams"> Paths for input and output files </param>
        /// <param name="operation"> Process operation type </param>
        public void CreateTasks(FilePaths pathParams, IOperation operation)
        {
            Guard.NotNull(pathParams, $"{nameof(pathParams)}");
            Guard.NotNull(operation, $"{nameof(operation)}");

            var fileChunks = operation.SplitFile(pathParams.InputFilePath)
                .OrderBy(x => x.StartPosition);

            // Each task has it's number for operations which could required sequential processing
            var taskNumber = 0;
            foreach (var chunk in fileChunks)
            {
                var syncParams = _taskSyncParamsFactory.Create(taskNumber, ResetEvent, CanProceed);
                var task = _gzipTaskFactory.Create(pathParams, syncParams, chunk, operation);

                _taskQueue.Enqueue(task);
                taskNumber++;
            }
        }
    }
}
