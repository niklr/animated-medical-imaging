using AMI.Core.Entities.Models;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// The API configuration.
    /// </summary>
    public interface IApiConfiguration
    {
        /// <summary>
        /// Gets the name of header used to identify the IP address of the connecting client.
        /// </summary>
        string ConnectingIpHeaderName { get; }

        /// <summary>
        /// Gets a value indicating whether the current environment is development.
        /// </summary>
        bool IsDevelopment { get; }

        /// <summary>
        /// Converts the API configuration to a model.
        /// </summary>
        /// <returns>The model represeting the API configuration.</returns>
        ApiSettings ToModel();
    }
}
