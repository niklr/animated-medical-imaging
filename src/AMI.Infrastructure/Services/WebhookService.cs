using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;
using Entities = AMI.Core.Entities;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The webhook service.
    /// </summary>
    public class WebhookService : IWebhookService
    {
        private readonly ILogger<WebhookService> logger;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public WebhookService(ILoggerFactory loggerFactory, IMediator mediator)
        {
            logger = loggerFactory?.CreateLogger<WebhookService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(string webhookId, string eventId, CancellationToken ct = default)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(webhookId, nameof(webhookId));
            Ensure.ArgumentNotNullOrWhiteSpace(eventId, nameof(eventId));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            var webhookModel = mediator.Send(
                new Entities.Webhooks.Queries.GetById.GetByIdQuery()
                {
                    Id = webhookId
                }, ct);
            if (webhookModel == null)
            {
                throw new UnexpectedNullException("The webhook could not be retrieved.");
            }

            var eventModel = mediator.Send(
                new Entities.Events.Queries.GetById.GetByIdQuery()
                {
                    Id = eventId
                }, ct);
            if (eventModel == null)
            {
                throw new UnexpectedNullException("The event could not be retrieved.");
            }

            await Task.CompletedTask;

            throw new Exception($"WebhookService.ProcessAsync with WebhookId: '{webhookId}' and EventId: '{eventId}' failed.");
        }
    }
}
