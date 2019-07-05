namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface to represent the response when the quota has been exceeded.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IQuotaExceededResponse
    {
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        int? StatusCode { get; }
    }
}
