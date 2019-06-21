namespace AMI.Core.IO.Models
{
    /// <summary>
    /// Represents a compressible item.
    /// </summary>
    public interface ICompressibleData
    {
        /// <summary>
        /// Gets the relative path of the file when compressed.
        /// </summary>
        /// <example>
        /// /Dir1/Dir2/MyFile.tmp
        /// </example>
        string CompressedDataRelativePathName { get; }
    }
}
