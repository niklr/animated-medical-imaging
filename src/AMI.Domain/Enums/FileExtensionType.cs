namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the file extension.
    /// </summary>
    public enum FileExtensionType
    {
        /// <summary>
        /// The type of the file extension is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The default file extension type.
        /// </summary>
        Default = 1,

        /// <summary>
        /// The combined file extension type representing the header.
        /// </summary>
        Header = 2,

        /// <summary>
        /// The combined file extension type representing the source.
        /// </summary>
        Source = 3
    }
}
