using System;
using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Workers;
using AMI.Domain.Enums;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Workers
{
    [TestFixture]
    public class TaskWorkerTests : BaseTest
    {
        [Test]
        public void TaskWorker_DoWorkAsync()
        {
            // Arrange
            var worker = GetService<ITaskWorker>();
            var queue = GetService<ITaskQueue>();
            var mediator = GetService<IMediator>();
            var context = GetService<IAmiUnitOfWork>();
            var command = new CreateTaskCommand()
            {
                Command = new ProcessObjectCommand()
                {
                    Id = Guid.NewGuid().ToString(),
                    AmountPerAxis = 1,
                    DesiredSize = 0
                }
            };
            var cts = new CancellationTokenSource();
            cts.CancelAfter(3000);

            // Act
            var result1 = mediator.Send(command, cts.Token).Result;
            worker.StartAsync(cts.Token);
            var result2 = context.TaskRepository.GetFirstOrDefault(e => e.Id == Guid.Parse(result1.Id));

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(TaskStatus.Queued, result1.Status);
            Assert.AreEqual(CommandType.ProcessObjectCommand, result1.Command.CommandType);
            Assert.AreEqual(WorkerStatus.Terminated, worker.WorkerStatus);
            Assert.IsNotNull(result2);
            Assert.AreEqual(TaskStatus.Failed, (TaskStatus)result2.Status);
            Assert.AreEqual("Unexpected null exception. ObjectEntity not found.", result2.Message);
        }
    }
}
