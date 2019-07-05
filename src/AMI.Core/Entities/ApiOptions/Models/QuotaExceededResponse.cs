using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The response when the quota has been exceeded.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class QuotaExceededResponse : IQuotaExceededResponse
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public int? StatusCode { get; set; } = 429;
    }
}
