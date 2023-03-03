using System;
using System.Threading;

namespace GZip.Logic.TaskManagement
{
    /// <summary> Creation of task synchronization params objects </summary>
    public interface ITaskSynchronizationParamsFactory
    {
        /// <summary> Creation of task synchronization params object </summary>
        /// <returns> Created object </returns>
        TaskSynchronizationParams Create(int taskNumber, ManualResetEventSlim resetEvent,
            Func<int, bool> canProceedFunc);
    }
}
