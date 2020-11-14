using FileProcessor.Configuration;
using FileProcessor.Logic.Threading;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace FileProcessor.Test.Threading
{
    public class ThreadManagerTests
    {
        private readonly ThreadManager target;
        private readonly Mock<IThreadProvider> threadProvider = new Mock<IThreadProvider>();
        private readonly Mock<IAppConfig> appConfig = new Mock<IAppConfig>();
        private readonly Mock<IThreadExecutable> taskManager = new Mock<IThreadExecutable>();

        public ThreadManagerTests()
        {
            target = new ThreadManager(threadProvider.Object, appConfig.Object);
        }

        [Fact]
        public void ExecuteInParallelTest_WorksFine()
        {
            // arrange
            Action<Exception> exceptionHandler = (ex) => { };

            // expectations
            appConfig.SetupGet(_ => _.ProcessorsCount).Returns(2);

            threadProvider.Setup(_ => _.CreateThread(It.IsAny<Action>()))
                .Callback<Action>(action => action());

            taskManager.Setup(_ => _.Execute(It.IsAny<Action<Exception>>()))
                .Callback<Action<Exception>>(action =>
                {
                    action.Should().BeEquivalentTo(exceptionHandler);
                });

            // act
            target.ExecuteInParallel(taskManager.Object, exceptionHandler);

            // assert
            appConfig.VerifyAll();
            threadProvider.VerifyAll();
            taskManager.VerifyAll();
        }

        [Fact]
        public void InterruptAllParallelExecutingThreadsTest_WorksFine()
        {
            // expectations
            threadProvider.Setup(_ => _.AbortAllThreads());

            // act
            target.InterruptAllParallelExecutingThreads();

            // assert
            threadProvider.VerifyAll();
        }
    }
}
