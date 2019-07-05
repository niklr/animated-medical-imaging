using AMI.Core.Entities.Models;
using Microsoft.Extensions.Options;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// The application configuration.
    /// </summary>
    /// <seealso cref="IAppConfiguration" />
    public class AppConfiguration : BaseConfiguration<AppOptions>, IAppConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfiguration"/> class.
        /// </summary>
        /// <param name="options">The application options.</param>
        public AppConfiguration(IOptions<AppOptions> options)
            : base(options)
        {
            Options = base.Options;
        }

        /// <inheritdoc/>
        public new IAppOptions Options { get; private set; }
    }
}
