using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Services;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The webhook service.
    /// </summary>
    public class WebhookService : IWebhookService
    {
        /// <inheritdoc/>
        public async Task ProcessAsync(string id, string eventId, CancellationToken ct = default)
        {
            await Task.CompletedTask;

            throw new Exception($"WebhookService.ProcessAsync with WebhookId: '{id}' and EventId: '{eventId}' failed.");
        }
    }
}
