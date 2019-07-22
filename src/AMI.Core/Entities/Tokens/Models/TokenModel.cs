using System;
using AMI.Core.Entities.Shared.Models;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing a token.
    /// </summary>
    public class TokenModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the token.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last used date.
        /// </summary>
        public DateTime LastUsedDate { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public string TokenValue { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static TokenModel Create(TokenEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TokenModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                LastUsedDate = entity.LastUsedDate,
                TokenValue = entity.TokenValue,
                UserId = entity.UserId.ToString()
            };
        }
    }
}
