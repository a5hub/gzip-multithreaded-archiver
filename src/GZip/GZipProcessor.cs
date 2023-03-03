using FileProcessor.Common;
using FileProcessor.Logic.Operations;
using FileProcessor.Logic.TaskManagement;
using FileProcessor.Logic.Threading;
using FileProcessor.Models;
using Serilog;
using System;
using System.Collections.Generic;
using GZip;

namespace FileProcessor
{
    /// <summary> Logic for file compression/decompression </summary>
    public class GZipProcessor : IGZipProcessor
    {
        private Dictionary<string, OperationType> operations = new Dictionary<string, OperationType>
        {
            {"compress", OperationType.Compress},
            {"decompress", OperationType.Decompress}
        };

        /// <summary> Manages all threads in application </summary>
        private readonly IThreadManager _threadManager;

        /// <summary> Tasks creating and executing logic </summary>
        private readonly ITaskManager _taskManager;

        /// <summary> Creates operation implementation dependent upon type </summary>
        private readonly IOperationFactory _operationFactory;

        /// <summary> Logic for file compression/decompression </summary>
        public GZipProcessor(IThreadManager threadManager,
            ITaskManager taskManager,
            IOperationFactory operationFactory)
        {
            _threadManager = threadManager;
            _taskManager = taskManager;
            _operationFactory = operationFactory;
        }

        /// <summary> GZip processing entry point </summary>
        /// <param name="args"> Console input </param>
        public void Run(string[] args)
        {
            Guard.NotNull(args, $"{nameof(args)}");
            Guard.True(args.Length > 0, $"{Resources.MainInputParametersEmpty}");

            // Check input and prepare initial params for processing start
            OperationType operationType;

            if (!operations.TryGetValue(args[0], out operationType))
            {
                Console.WriteLine(Resources.MainWrongOperationMessage);
                return;
            }

            var pathParams = new FilePaths
            {
                InputFilePath = args[1],
                OutputFilePath = args[2]
            };

            Console.WriteLine(Resources.MainProcessStartedMessage, DateTime.Now);

            Log.Information(
                $"File processing started input file {pathParams.InputFilePath}, output file {pathParams.OutputFilePath}, operation {args[0]}");

            // Execute processing
            ProcessFile(pathParams, operationType);

            Log.Information("File processing finished.");
            Console.WriteLine(Resources.MainProcessFinishedMessage, DateTime.Now);
        }

        /// <summary> Compress or decompress file </summary>
        /// <param name="pathParams"> Paths for input and output files </param>
        /// <param name="operationType"> Process operation type </param>
        public void ProcessFile(FilePaths pathParams, OperationType operationType)
        {
            Guard.NotNull(pathParams, $"{nameof(FilePaths)}");
            Guard.DefinedEnumValue(operationType);
            Guard.True(operationType != OperationType.Default, 
                Resources.UnexpectedOperationException);

            var operation = _operationFactory.Create(operationType);

            _taskManager.CreateTasks(pathParams, operation);
            _threadManager.ExecuteInParallel(_taskManager, UnexpectedTerminationProc);
        }

        /// <summary> Method which will be call in case of thread exception </summary>
        /// <param name="exception"> Internal thread exception </param>
        public void UnexpectedTerminationProc(Exception exception)
        {
            Guard.NotNull(exception, $"{nameof(exception)}");

            _threadManager.InterruptAllParallelExecutingThreads();
            Console.WriteLine(exception.Message);
            EnvironmentControl.Current.Exit(0);
        }
    }
}
