namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the file result.
    /// </summary>
    public class FileResultModel
    {
        /// <summary>
        /// Gets or sets the file contents.
        /// </summary>
        public byte[] FileContents { get; set; }

        /// <summary>
        /// Gets or sets the Content-Type of the file.
        /// </summary>
        public string ContentType { get; set; }
    }
}
