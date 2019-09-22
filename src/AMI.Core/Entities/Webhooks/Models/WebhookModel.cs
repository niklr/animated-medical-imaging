using System;
using AMI.Core.Constants;
using AMI.Core.Entities.Shared.Models;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the webhook endpoint.
    /// </summary>
    public class WebhookModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the webhook endpoint.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the URL of the webhook endpoint.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the API version used to render events.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the enabled events for this endpoint.
        /// Specify ['*'] to enable all events.
        /// </summary>
        public string[] EnabledEvents { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="constants">The application constants.</param>
        /// <returns>The domain entity as a model.</returns>
        public static WebhookModel Create(WebhookEntity entity, IApplicationConstants constants)
        {
            if (entity == null)
            {
                return null;
            }

            return new WebhookModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                Url = entity.Url,
                ApiVersion = entity.ApiVersion,
                EnabledEvents = string.IsNullOrWhiteSpace(entity.EnabledEvents) ?
                    Array.Empty<string>() : entity.EnabledEvents.Replace(constants.ValueSeparator, string.Empty).Split(','),
                UserId = entity.UserId
            };
        }
    }
}
