using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// The result of a file extension mapping.
    /// </summary>
    public class FileExtensionMappingResult
    {
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the type of the file extension.
        /// </summary>
        public FileExtensionType FileExtensionType { get; set; }

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public FileFormat FileFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mapped file extension represents an archive.
        /// </summary>
        public bool IsArchive { get; set; }
    }
}
