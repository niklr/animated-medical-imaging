namespace AMI.Core.IO.Models
{
    /// <summary>
    /// Represents an item at a specified location.
    /// </summary>
    public interface ICompressibleDataLocation : ICompressibleData
    {
        /// <summary>
        /// Gets the path to the file on the disk.
        /// </summary>
        string DiskFilePath { get; }
    }
}
