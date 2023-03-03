using System;
using System.Collections.Generic;
using System.Threading;
using GZip.Logic.Threading;
using Moq;
using Xunit;

namespace GZip.Tests.Threading
{
    public class ThreadProviderTests
    {
        private readonly ThreadProvider target;

        public ThreadProviderTests()
        {
            target = new ThreadProvider();
        }

        [Fact]
        public void CreateThreadTest_WorksFine()
        {
            // act
            target.CreateThread(null);

            // assert
            Assert.True(target.Threads.Count == 1);
        }

        [Fact]
        public void WaitAllThreadsTest_WorksFine()
        {
            // arrange
            Action method = () => { Thread.Sleep(100); };
            var thread1 = new Thread(() => method());
            var thread2 = new Thread(() => method());
            var threads = new List<Thread> {thread1, thread2};
            thread1.Start();
            thread2.Start();

            // expectations
            var mock = new Mock<ThreadProvider>();
            mock.SetupGet(x => x.Threads).Returns(threads);

            // act
            mock.Object.WaitAllThreads();

            // assert
            Assert.Equal(threads, mock.Object.Threads);
            mock.VerifyAll();
        }

        [Fact]
        public void StartAllThreadsTest_WorksFine()
        {
            // arrange
            Action method = () => { Thread.Sleep(100); };
            var thread1 = new Thread(() => method());
            var thread2 = new Thread(() => method());
            var threads = new List<Thread> {thread1, thread2};

            // expectations
            var mock = new Mock<ThreadProvider>();
            mock.SetupGet(x => x.Threads).Returns(threads);

            // act
            mock.Object.StartAllThreads();

            // assert
            Assert.Equal(threads, mock.Object.Threads);
            mock.VerifyAll();
        }

        [Fact]
        public void AbortAllThreadsTest_WorksFine()
        {
            // arrange
            Action method = () => { Thread.Sleep(100); };
            var thread1 = new Thread(() => method());
            var thread2 = new Thread(() => method());
            var threads = new List<Thread> {thread1, thread2};

            // expectations
            var mock = new Mock<ThreadProvider>();
            mock.SetupGet(x => x.Threads).Returns(threads);

            // act
            mock.Object.AbortAllThreads();

            // assert
            Assert.Equal(threads, mock.Object.Threads);
            mock.VerifyAll();
        }
    }
}
