using System;
using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Webhooks.Commands.Create;
using AMI.Core.Entities.Webhooks.Commands.Delete;
using AMI.Core.Repositories;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Webhooks.Commands
{
    [TestFixture]
    public class DeleteWebhookCommandTests : BaseTest
    {
        [Test]
        public void DeleteWebhookCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
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
            var result1 = mediator.Send(command1, ct).Result;
            var entity1 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result1.Id));
            var command2 = new DeleteWebhookCommand()
            {
                Id = result1.Id
            };

            // Act
            var result2 = mediator.Send(command2, ct).Result;
            var entity2 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result1.Id));

            // Assert
            Assert.IsNotNull(entity1);
            Assert.IsTrue(result2);
            Assert.IsNull(entity2);
        }
    }
}
