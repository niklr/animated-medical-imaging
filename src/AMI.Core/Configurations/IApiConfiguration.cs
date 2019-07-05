using AMI.Core.Entities.Models;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// An interface representing the API configuration.
    /// </summary>
    public interface IApiConfiguration : IBaseConfiguration<ApiOptions>
    {
        /// <summary>
        /// Gets the API options.
        /// </summary>
        IApiOptions Options { get; }
    }
}
