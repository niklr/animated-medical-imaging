using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Events.Commands.Create;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The event service.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> logger;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public EventService(ILoggerFactory loggerFactory, IMediator mediator)
        {
            logger = loggerFactory?.CreateLogger<EventService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        public async Task<EventModel> CreateAsync<T>(string userId, EventType eventType, T data, CancellationToken ct)
        {
            try
            {
                // TODO: store the event and if at least one webhook exists create a background job
                var command = new CreateEventCommand()
                {
                    UserId = userId,
                    EventType = eventType,
                    Event = data
                };

                var result = await mediator.Send(command);

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
