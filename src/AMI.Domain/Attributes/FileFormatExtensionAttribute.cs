using System;
using AMI.Domain.Enums;

namespace AMI.Domain.Attributes
{
    /// <summary>
    /// An attribute used to annotate enums representing file formats with the corresponding extension.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class FileFormatExtensionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets or sets the type of the file extension.
        /// </summary>
        public FileExtensionType FileExtensionType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormatExtensionAttribute"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="fileExtensionType">Type of the file extension.</param>
        public FileFormatExtensionAttribute(string extension, FileExtensionType fileExtensionType = FileExtensionType.Default)
        {
            Extension = extension?.Trim();
            FileExtensionType = fileExtensionType;
        }
    }
}
