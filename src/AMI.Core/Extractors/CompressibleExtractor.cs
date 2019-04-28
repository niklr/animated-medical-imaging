using AMI.Core.Configuration;

namespace AMI.Core.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    public abstract class CompressibleExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressibleExtractor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CompressibleExtractor(IAmiConfigurationManager configuration)
        {
        }

        /// <summary>
        /// Gets the maximum of compressible entries.
        /// </summary>
        public uint MaxCompressibleEntries { get; private set; } = uint.MinValue;
    }
}
