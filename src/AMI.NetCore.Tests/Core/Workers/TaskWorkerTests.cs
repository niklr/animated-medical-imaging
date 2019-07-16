using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Workers;
using AMI.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Workers
{
    [TestFixture]
    public class TaskWorkerTests : BaseTest
    {
        [Test]
        public async Task TaskWorker_DoWorkAsync()
        {
            // Arrange
            var pause = new ManualResetEvent(false);
            var loggerFactory = GetService<ILoggerFactory>();
            var queue = GetService<ITaskQueue>();
            var mediator = GetService<IMediator>();
            var context = GetService<IAmiUnitOfWork>();
            var worker = new TaskWorker(loggerFactory, mediator, queue);
            var command = new CreateTaskCommand()
            {
                Command = new ProcessObjectCommand()
                {
                    Id = Guid.NewGuid().ToString(),
                    AmountPerAxis = 1,
                    OutputSize = 0
                }
            };
            var cts = new CancellationTokenSource();
            cts.CancelAfter(3000);

            // Act
            var result1 = mediator.Send(command, cts.Token).Result;
            await worker.StartAsync(cts.Token);
            pause.WaitOne(4000);
            var result2 = context.TaskRepository.GetFirstOrDefault(e => e.Id == Guid.Parse(result1.Id));

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(Domain.Enums.TaskStatus.Queued, result1.Status);
            Assert.AreEqual(CommandType.ProcessObjectCommand, result1.Command.CommandType);
            Assert.AreEqual(WorkerType.Default, worker.WorkerType);
            Assert.AreEqual(WorkerStatus.Terminated, worker.WorkerStatus);
            Assert.IsNotNull(result2);
            Assert.AreEqual(Domain.Enums.TaskStatus.Failed, (Domain.Enums.TaskStatus)result2.Status);
            Assert.AreEqual("Unexpected null exception. ObjectEntity not found.", result2.Message);
        }
    }
}
