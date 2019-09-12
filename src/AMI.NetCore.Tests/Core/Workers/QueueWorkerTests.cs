using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Workers;
using AMI.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Workers
{
    [TestFixture]
    public class QueueWorkerTests : BaseTest
    {
        [Test]
        public async Task QueueWorker_DoWorkAsync()
        {
            // Arrange
            var pause = new ManualResetEvent(false);
            var loggerFactory = GetService<ILoggerFactory>();
            var workerService = GetService<IWorkerService>();
            var gateway = GetService<IGatewayService>();
            var configuration = GetService<IAppConfiguration>();
            var mediator = GetService<IMediator>();
            var queue = GetService<ITaskQueue>();
            var context = GetService<IAmiUnitOfWork>();
            var worker = new QueueWorker(loggerFactory, workerService, gateway, configuration, queue, ServiceProvider);
            var cts = new CancellationTokenSource();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            string dataPath = GetDataPath(filename);
            var command1 = new CreateObjectCommand()
            {
                OriginalFilename = filename,
                SourcePath = CreateTempFile(dataPath)
            };
            var result1 = mediator.Send(command1, cts.Token).Result;
            var command2 = new CreateTaskCommand()
            {
                Command = new ProcessObjectCommand()
                {
                    Id = result1.Id,
                    AmountPerAxis = 1,
                    OutputSize = 0
                }
            };

            // Act
            var result2 = mediator.Send(command2, cts.Token).Result;
            cts.CancelAfter(500);
            await worker.StartAsync(cts.Token);
            pause.WaitOne(1000);
            var result3 = context.TaskRepository.GetFirstOrDefault(e => e.Id == Guid.Parse(result2.Id));

            // Assert
            Assert.IsNotNull(result2);
            Assert.AreEqual(Domain.Enums.TaskStatus.Queued, result2.Status);
            Assert.AreEqual(CommandType.ProcessObjectCommand, result2.Command.CommandType);
            Assert.AreEqual(WorkerType.Queue, worker.WorkerType);
            Assert.AreEqual(WorkerStatus.Terminated, worker.WorkerStatus);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(Domain.Enums.TaskStatus.Queued, (Domain.Enums.TaskStatus)result3.Status);
            Assert.AreNotEqual(Domain.Enums.TaskStatus.Processing, (Domain.Enums.TaskStatus)result3.Status);
        }
    }
}
