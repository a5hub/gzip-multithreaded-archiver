using GZip.Common.Common;

namespace GZip.Logic.Logic.TaskManagement;

/// <summary> Creation of task synchronization params objects </summary>
public class TaskSynchronizationParamsFactory : ITaskSynchronizationParamsFactory
{
    /// <summary> Creation of task synchronization params object </summary>
    /// <returns> Created object </returns>
    public TaskSynchronizationParams Create(int taskNumber, ManualResetEventSlim resetEvent,
        Func<int, bool> canProceedFunc)
    {
        Guard.NotNull(resetEvent, $"{nameof(resetEvent)}");
        Guard.NotNull(canProceedFunc, $"{nameof(canProceedFunc)}");

        return new TaskSynchronizationParams
        {
            TaskNumber = taskNumber,
            ResetEvent = resetEvent,
            CanProceedFunc = canProceedFunc
        };
    }
}