using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// An interface representing a mapper for file extensions.
    /// </summary>
    public interface IFileExtensionMapper
    {
        /// <summary>
        /// Maps specified path or filename to the corresponding extension.
        /// The file format is DICOM by default to support paths/filenames without extensions.
        /// </summary>
        /// <example>
        /// Providing "sample.mhd" as filename will return <see cref="FileFormat.MetaImage"/>, <see cref="FileExtensionType.Header"/> and .mhd as extension.
        /// </example>
        /// <param name="path">The path.</param>
        /// <returns>The result of the mapping.</returns>
        FileExtensionMappingResult Map(string path);

        /// <summary>
        /// Maps the specified path or filename to the corresponding counterpart.
        /// This method addresses combined file formats consisting of a header and a source file.
        /// </summary>
        /// <example>
        /// Providing "sample.mhd" as filename will return <see cref="FileFormat.MetaImage"/>, <see cref="FileExtensionType.Source"/> and .raw as extension.
        /// </example>
        /// <param name="path">The path.</param>
        /// <returns>The counterpart of the specified path/filename.</returns>
        FileExtensionMappingResult MapCounterpart(string path);
    }
}
