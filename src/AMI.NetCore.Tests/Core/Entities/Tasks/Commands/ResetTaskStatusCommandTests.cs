using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Tasks.Commands.ResetStatus;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Tasks.Commands
{
    [TestFixture]
    public class ResetTaskStatusCommandTests : BaseTest
    {
        [Test]
        public void ResetTaskStatusCommand_Send()
        {
            // Arrange
            var context = GetService<IAmiUnitOfWork>();
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new ResetTaskStatusCommand();
            var createdDate = DateTime.UtcNow;
            var createdTasks = CreateTasks(context, createdDate);

            try
            {
                // Act
                var result = mediator.Send(command, ct).Result;
                var entities = context.TaskRepository.GetQuery(e => 
                    e.CreatedDate == createdDate && e.Status == (int)Domain.Enums.TaskStatus.Queued);

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual(1000, entities.Count());
            }
            finally
            {
                context.TaskRepository.RemoveRange(createdTasks);
            }
        }

        private IList<TaskEntity> CreateTasks(IAmiUnitOfWork context, DateTime createdDate)
        {
            IList<TaskEntity> tasks = new List<TaskEntity>();

            for (int i = 0; i < 1000; i++)
            {
                var task = new TaskEntity()
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = createdDate,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Domain.Enums.TaskStatus.Processing
                };
                tasks.Add(task);
                context.TaskRepository.Add(task);
            }

            context.SaveChanges();

            var entities = context.TaskRepository.GetQuery(e => 
                e.CreatedDate == createdDate && e.Status == (int)Domain.Enums.TaskStatus.Processing);

            Assert.AreEqual(1000, entities.Count());

            return tasks;
        }
    }
}
