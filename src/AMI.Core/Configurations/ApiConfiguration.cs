using System;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace AMI.Core.Configurations
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
        public ApiConfiguration(IOptions<ApiSettings> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.Value == null)
            {
                throw new UnexpectedNullException(nameof(configuration), nameof(ApiSettings));
            }

            ConnectingIpHeaderName = configuration.Value.ConnectingIpHeaderName;
            IsDevelopment = configuration.Value.IsDevelopment;
        }

        /// <inheritdoc/>
        public string ConnectingIpHeaderName { get; private set; }

        /// <inheritdoc/>
        public bool IsDevelopment { get; private set; }

        /// <inheritdoc/>
        public ApiSettings ToModel()
        {
            return new ApiSettings()
            {
                ConnectingIpHeaderName = ConnectingIpHeaderName,
                IsDevelopment = IsDevelopment
            };
        }
    }
}
