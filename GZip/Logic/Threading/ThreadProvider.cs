using System;
using System.Collections.Generic;
using System.Threading;

namespace FileProcessor.Logic.Threading
{
    /// <summary> Provides base thread functional </summary>
    public class ThreadProvider : IThreadProvider
    {
        /// <summary> Amount of created threads </summary>
        public virtual List<Thread> Threads { get; } = new List<Thread>();

        /// <summary> Create a new thread </summary>
        /// <param name="method"> Method which will be executed in separated thread </param>
        public void CreateThread(Action method)
        {
            Threads.Add(new Thread(() => method()));
        }

        /// <summary> Wait all started threads execution</summary>
        public void WaitAllThreads()
        {
            foreach (var t in Threads)
            {
                t.Join();
            }
        }

        /// <summary> Start execution of all created threads </summary>
        public void StartAllThreads()
        {
            foreach (var t in Threads)
            {
                t.Start();
            }
        }

        /// <summary> Aborts execution of all created threads </summary>
        public void AbortAllThreads()
        {
            foreach (var t in Threads)
            {
                t.Interrupt();
            }
        }
    }
}
