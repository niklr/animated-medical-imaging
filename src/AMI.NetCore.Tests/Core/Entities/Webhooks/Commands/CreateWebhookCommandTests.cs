using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMI.Core.Entities.Webhooks.Commands.Create;
using AMI.Core.Repositories;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Webhooks.Commands
{
    [TestFixture]
    public class CreateWebhookCommandTests : BaseTest
    {
        [Test]
        public void CreateWebhookCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
            var command = new CreateWebhookCommand()
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

            // Act
            var result1 = mediator.Send(command, ct).Result;
            var result2 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result1.Id));

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(command.ApiVersion, result1.ApiVersion);
            Assert.AreEqual(2, result1.EnabledEvents.Length);
            Assert.AreEqual("TaskUpdated", result1.EnabledEvents[0]);
            Assert.AreEqual("TaskCreated", result1.EnabledEvents[1]);
            Assert.AreEqual(command.Url, result1.Url);

            Assert.IsNotNull(result2);
            Assert.AreEqual("#TaskUpdated#,#TaskCreated#", result2.EnabledEvents);
        }

        [Test]
        public void CreateWebhookCommand_Validation_Failures_1()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateWebhookCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(4, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.ApiVersion)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Api Version' must not be empty.", firstEntry[0]);
        }

        [Test]
        public void CreateWebhookCommand_Validation_Failures_2()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateWebhookCommand()
            {
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
        }

        [Test]
        public void CreateWebhookCommand_Validation_Failures_3()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateWebhookCommand()
            {
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
    }
}
