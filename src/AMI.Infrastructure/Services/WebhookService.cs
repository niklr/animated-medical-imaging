using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.IO.Clients;
using AMI.Core.Services;
using AMI.Core.Wrappers;
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
        private readonly IApiConfiguration configuration;
        private readonly IJsonHttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public WebhookService(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IApiConfiguration configuration,
            IJsonHttpClient httpClient)
        {
            logger = loggerFactory?.CreateLogger<WebhookService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(string webhookId, string eventId, IWrappedJobCancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(webhookId, nameof(webhookId));
            Ensure.ArgumentNotNullOrWhiteSpace(eventId, nameof(eventId));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct.ShutdownToken);
            if (configuration?.Options?.RequestTimeoutMilliseconds > 0)
            {
                cts.CancelAfter(configuration.Options.RequestTimeoutMilliseconds);
            }

            var webhookModel = await mediator.Send(
                new Entities.Webhooks.Queries.GetById.GetByIdQuery()
                {
                    Id = webhookId
                }, cts.Token);
            if (webhookModel == null)
            {
                throw new UnexpectedNullException("The webhook could not be retrieved.");
            }

            var eventModel = await mediator.Send(
                new Entities.Events.Queries.GetById.GetByIdQuery()
                {
                    Id = eventId
                }, cts.Token);
            if (eventModel == null)
            {
                throw new UnexpectedNullException("The event could not be retrieved.");
            }

            // TODO: set signature in the header
            var response = await httpClient.PostAsync(webhookModel.Url, eventModel, cts.Token);
            if (response == null)
            {
                throw new UnexpectedNullException("The webhook response is null.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new AmiException($"The webhook response is not successful '{response.StatusCode.ToString()}'. " +
                    $"WebhookId: {webhookId} EventId: {eventId}");
            }
        }
    }
}
