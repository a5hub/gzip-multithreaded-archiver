using System;
using System.Threading;

namespace FileProcessor.Logic.TaskManagement
{
    /// <summary> Parameters needed to organize thread task synchronization </summary>
    public class TaskSynchronizationParams
    {
        /// <summary> Sequential task number </summary>
        /// <remarks> If some parts required sequential processing </remarks>
        public int TaskNumber { get; set; }

        /// <summary> Other threads tasks signaler </summary>
        public ManualResetEventSlim ResetEvent { get; set; }

        /// <summary> Callback function to check if task can proceed </summary>
        public Func<int, bool> CanProceedFunc { get; set; }
    }
}
