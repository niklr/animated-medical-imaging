using System;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Services;
using AMI.Hangfire.Wrappers;
using Hangfire;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Proxies
{
    /// <summary>
    /// A proxy to the service handling webhook events.
    /// </summary>
    public class WebhookServiceProxy
    {
        private readonly IWebhookService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookServiceProxy"/> class.
        /// </summary>
        /// <param name="service">The webhook service.</param>
        public WebhookServiceProxy(IWebhookService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Processes the webhook event asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the webhook.</param>
        /// <param name="eventId">The identifier of the event.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Queue(QueueNames.Webhooks)]
        [AutomaticRetry(Attempts = 5)]
        public async Task ProcessAsync(string id, string eventId, IJobCancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(id, nameof(id));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            await service.ProcessAsync(id, eventId, new JobCancellationTokenWrapper(ct));
        }
    }
}
