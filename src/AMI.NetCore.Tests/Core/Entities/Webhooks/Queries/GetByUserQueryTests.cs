using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMI.Core.Entities.Webhooks.Commands.Create;
using AMI.Core.Entities.Webhooks.Queries.GetByUser;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using AMI.NetCore.Tests.Mocks.Core;
using MediatR;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMI.NetCore.Tests.Core.Entities.Webhooks.Queries
{
    [TestFixture]
    public class GetByUserQueryTests : BaseTest
    {
        [TestCase(SHARED_GUID_1, EventType.Unknown, 2)]
        [TestCase(SHARED_GUID_1, EventType.TaskUpdated, 2)]
        [TestCase(SHARED_GUID_1, EventType.ObjectCreated, 1)]
        [TestCase(SHARED_GUID_2, EventType.Unknown, 1)]
        [TestCase(SHARED_GUID_2, EventType.TaskCreated, 1)]
        [TestCase(SHARED_GUID_2, EventType.ObjectCreated, 0)]
        public void GetObjectByUserQuery(string userId, EventType eventType, int expectedWebhooksCount)
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var uow = GetService<IAmiUnitOfWork>();
            var ct = new CancellationToken();
            var entities = CreateWebhooks(mediator, uow, ct);
            var query = new GetByUserQuery()
            {
                UserId = userId,
                EventType = eventType
            };

            try
            {
                // Act
                var result = mediator.Send(query, ct).Result;

                // Assert
                Assert.AreEqual(expectedWebhooksCount, result.Count());
            }
            finally
            {
                foreach (var entity in entities)
                {
                    uow.WebhookRepository.Remove(entity);
                }
                uow.SaveChanges();
            }
        }

        private IList<WebhookEntity> CreateWebhooks(IMediator mediator, IAmiUnitOfWork uow, CancellationToken ct)
        {
            IList<WebhookEntity> entities = new List<WebhookEntity>();

            // Webhook 1
            var command1 = new CreateWebhookCommand()
            {
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    EventType.TaskUpdated.ToString(),
                    EventType.TaskCreated.ToString()
                },
                Secret = "1234",
                Url = "http://localhost/webhook1"
            };
            var result1 = mediator.Send(command1, ct).Result;
            var entity1 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result1.Id));
            entities.Add(entity1);

            // Webhook 2
            var command2 = new CreateWebhookCommand()
            {
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    "*"
                },
                Secret = "1234",
                Url = "http://localhost/webhook2"
            };
            var result2 = mediator.Send(command2, ct).Result;
            var entity2 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result2.Id));
            entities.Add(entity2);

            // Change user
            var principal1 = TestExecutionContext.CurrentContext.CurrentPrincipal;
            TestExecutionContext.CurrentContext.CurrentPrincipal = new MockPrincipal(
                SHARED_GUID_2, new RoleType[] { RoleType.Administrator });

            // Webhook 3
            var command3 = new CreateWebhookCommand()
            {
                ApiVersion = "1.0.0",
                EnabledEvents = new HashSet<string>()
                {
                    EventType.TaskUpdated.ToString(),
                    EventType.TaskCreated.ToString()
                },
                Secret = "1234",
                Url = "http://localhost/webhook3"
            };
            var result3 = mediator.Send(command3, ct).Result;
            var entity3 = uow.WebhookRepository.GetFirstOrDefault(e => e.Id == new Guid(result3.Id));
            entities.Add(entity3);

            // Change user
            TestExecutionContext.CurrentContext.CurrentPrincipal = principal1;

            return entities;
        }

        [Test]
        public void GetObjectByUserQuery_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var query = new GetByUserQuery();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(query, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(1, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(query.UserId)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'User Id' must not be empty.", firstEntry[0]);
        }
    }
}
