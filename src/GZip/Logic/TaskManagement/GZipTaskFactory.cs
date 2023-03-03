﻿using FileProcessor.Common;
using FileProcessor.Logic.Operations;
using FileProcessor.Models;

namespace FileProcessor.Logic.TaskManagement
{
    /// <summary> Creation of tasks objects </summary>
    public class GZipTaskFactory : IGZipTaskFactory
    {
        /// <summary> Creation of task object </summary>
        /// <returns> Created object </returns>
        public GZipTask Create(FilePaths pathParams, TaskSynchronizationParams syncParams,
            FileChunk chunk, IOperation operation)
        {
            Guard.NotNull(pathParams, $"{nameof(pathParams)}");
            Guard.NotNull(syncParams, $"{nameof(syncParams)}");
            Guard.NotNull(chunk, $"{nameof(chunk)}");
            Guard.NotNull(operation, $"{nameof(operation)}");

            return new GZipTask(pathParams, syncParams, chunk, operation);
        }
    }
}
