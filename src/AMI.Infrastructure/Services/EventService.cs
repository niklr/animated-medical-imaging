using System;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task CreateAsync<T>(string userId, EventType eventType, T data, CancellationToken ct)
        {
            // TODO: store the event and if at least one webhook exists create a background job
            await Task.CompletedTask;
        }
    }
}
