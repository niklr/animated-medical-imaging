using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Webhooks.Commands.Create;
using AMI.Core.Entities.Webhooks.Commands.Update;
using AMI.Core.Repositories;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Webhooks.Commands
{
    [TestFixture]
    public class UpdateWebhookCommandTests : BaseTest
    {
        [Test]
        public void UpdateWebhookCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
            var result1 = Create(mediator, ct);

            // Act
            var command2 = new UpdateWebhookCommand()
            {
                Id = result1.Id,
                ApiVersion = "2.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    "TaskUpdated",
                    "TaskCreated",
                    "TaskDeleted"
                },
                Secret = "4321",
                Url = "http://localhost/webhook/v2"
            };
            var result2 = mediator.Send(command2, ct).Result;
            var entity = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result1.Id));

            // Assert
            Assert.IsNotNull(result2);
            Assert.AreEqual(command2.ApiVersion, result2.ApiVersion);
            Assert.AreEqual(3, result2.EnabledEvents.Length);
            Assert.AreEqual("TaskUpdated", result2.EnabledEvents[0]);
            Assert.AreEqual("TaskCreated", result2.EnabledEvents[1]);
            Assert.AreEqual("TaskDeleted", result2.EnabledEvents[2]);
            Assert.AreEqual(command2.Url, result2.Url);

            Assert.IsNotNull(entity);
            Assert.AreEqual("#TaskUpdated#,#TaskCreated#,#TaskDeleted#", entity.EnabledEvents);

            uow.WebhookRepository.Remove(entity);
        }

        [Test]
        public void UpdateWebhookCommand_Validation_Failures_1()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new UpdateWebhookCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(5, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.ApiVersion)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Api Version' must not be empty.", firstEntry[0]);
        }

        [Test]
        public void UpdateWebhookCommand_Validation_Failures_2()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
            var result = Create(mediator, ct);
            var entity = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result.Id));
            var command = new UpdateWebhookCommand()
            {
                Id = result.Id,
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    "Test"
                },
                Secret = "1234",
                Url = "http://localhost/webhook"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(1, ex.Failures.Count);
            var firstEntry = ex.Failures.FirstOrDefault().Value;
            Assert.AreEqual(1, firstEntry.Length);
            Assert.IsTrue(firstEntry[0].StartsWith("'Enabled Events' must be one of these values: Unknown, TaskCr"));

            uow.WebhookRepository.Remove(entity);
        }

        [Test]
        public void UpdateWebhookCommand_Validation_Failures_3()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
            var result = Create(mediator, ct);
            var entity = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result.Id));
            var command = new UpdateWebhookCommand()
            {
                Id = result.Id,
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    "Unknown"
                },
                Secret = "1234",
                Url = "localhost/webhook"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(1, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.Url)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("The specified condition was not met for 'Url'.", firstEntry[0]);
        }

        private WebhookModel Create(IMediator mediator, CancellationToken ct)
        {
            var command1 = new CreateWebhookCommand()
            {
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    "TaskUpdated",
                    "TaskCreated"
                },
                Secret = "1234",
                Url = "http://localhost/webhook"
            };

            return mediator.Send(command1, ct).Result;
        }
    }
}
