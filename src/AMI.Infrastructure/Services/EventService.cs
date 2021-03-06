﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Events.Commands.Create;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Webhooks.Queries.GetByUser;
using AMI.Core.Services;
using AMI.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The event service.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> logger;
        private readonly IMediator mediator;
        private readonly IBackgroundService backgroundService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="backgroundService">The background service.</param>
        public EventService(ILoggerFactory loggerFactory, IMediator mediator, IBackgroundService backgroundService)
        {
            logger = loggerFactory?.CreateLogger<EventService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.backgroundService = backgroundService ?? throw new ArgumentNullException(nameof(backgroundService));
        }

        /// <inheritdoc/>
        public async Task<EventModel> CreateAsync<T>(string userId, EventType eventType, T data, CancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(userId, nameof(userId));
            Ensure.ArgumentNotNull(data, nameof(data));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            try
            {
                var command = new CreateEventCommand()
                {
                    UserId = userId,
                    EventType = eventType,
                    Event = data
                };

                var result = await mediator.Send(command, ct);

                var query = new GetByUserQuery()
                {
                    UserId = userId,
                    EventType = eventType
                };

                // create a background job for each webhook
                var webhooks = await mediator.Send(query, ct);
                if (webhooks != null && webhooks.Count() > 0)
                {
                    foreach (var webhook in webhooks)
                    {
                        if (webhook != null)
                        {
                            backgroundService.EnqueueWebhookEvent(webhook.Id, result.Id);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return null;
        }
    }
}
