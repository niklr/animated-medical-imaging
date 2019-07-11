namespace AMI.Core.Mappers
{
    /// <summary>
    /// An interface representing a mapper for file extensions.
    /// </summary>
    public interface IFileExtensionMapper
    {
        /// <summary>
        /// Maps specified filename to the corresponding extension.
        /// The file format is DICOM by default to support filenames without extensions.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The result of the mapping.</returns>
        FileExtensionMappingResult Map(string filename);
    }
}
