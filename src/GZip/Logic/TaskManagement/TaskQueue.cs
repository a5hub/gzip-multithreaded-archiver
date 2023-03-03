using System.Collections.Concurrent;

namespace GZip.Logic.TaskManagement
{
    /// <summary> Working queue for tasks to be processed </summary>
    public class TaskQueue : ITaskQueue<GZipTask>
    {
        /// <summary> Working queue for tasks to be processed </summary>
        public ConcurrentQueue<GZipTask> Queue { get; } = new();
        
        /// <summary> Returns true if queue no empty yet </summary>
        public bool NotEmpty()
        {
            return !Queue.IsEmpty;
        }

        /// <summary> Try to get element and delete it from queue </summary>
        /// <param name="result"> Element </param>
        public void TryDequeue(out GZipTask result)
        {
            Queue.TryDequeue(out result);
        }

        /// <summary> Put element into queue </summary>
        /// <param name="item"> Element </param>
        public void Enqueue(GZipTask item)
        {
            Queue.Enqueue(item);
        }
    }
}
