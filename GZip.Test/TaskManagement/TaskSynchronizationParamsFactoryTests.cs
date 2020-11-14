using System;
using FileProcessor.Logic.TaskManagement;
using System.Threading;
using Xunit;
using FluentAssertions;

namespace FileProcessor.Test.TaskManagement
{
    public class TaskSynchronizationParamsFactoryTests
    {
        private TaskSynchronizationParamsFactory target;

        public TaskSynchronizationParamsFactoryTests()
        {
            target = new TaskSynchronizationParamsFactory();
        }

        [Fact]
        public void CreateTest_WorksFine()
        {
            // arrange
            var taskNumber = 12;
            var resetEvent = new ManualResetEventSlim(false);
            Func<int, bool> func = (param) => { return true; };
            var expected = new TaskSynchronizationParams
            {
                TaskNumber = taskNumber,
                ResetEvent = resetEvent,
                CanProceedFunc = func
            };

            // act
            var actual = target.Create(taskNumber, resetEvent, func);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
