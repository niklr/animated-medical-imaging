using AMI.Core.Entities.Models;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// An interface representing the application configuration.
    /// </summary>
    public interface IAppConfiguration : IBaseConfiguration<AppOptions>
    {
        /// <summary>
        /// Gets the application options.
        /// </summary>
        IAppOptions Options { get; }
    }
}
