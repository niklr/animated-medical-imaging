using AMI.Core.Entities.Models;
using Microsoft.Extensions.Options;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// The API configuration.
    /// </summary>
    public class ApiConfiguration : BaseConfiguration<ApiOptions>, IApiConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfiguration"/> class.
        /// </summary>
        /// <param name="options">The API options.</param>
        public ApiConfiguration(IOptions<ApiOptions> options)
            : base(options)
        {
            Options = base.Options;
        }

        /// <inheritdoc/>
        public new IApiOptions Options { get; private set; }
    }
}
