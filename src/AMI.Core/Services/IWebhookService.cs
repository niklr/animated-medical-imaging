using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service to handle webhooks.
    /// </summary>
    public interface IWebhookService
    {
        /// <summary>
        /// Processes the webhook event asynchronous.
        /// </summary>
        /// <param name="webhookId">The identifier of the webhook.</param>
        /// <param name="eventId">The identifier of the event.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ProcessAsync(string webhookId, string eventId, CancellationToken ct = default);
    }
}
