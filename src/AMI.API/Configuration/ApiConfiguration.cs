using Microsoft.Extensions.Configuration;

namespace AMI.API.Configuration
{
    /// <summary>
    /// The API configuration.
    /// </summary>
    public class ApiConfiguration : IApiConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ApiConfiguration(IConfiguration configuration)
        {
        }
    }
}
