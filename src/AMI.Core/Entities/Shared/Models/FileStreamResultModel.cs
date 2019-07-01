using System.IO;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the file result.
    /// </summary>
    public class FileStreamResultModel
    {
        /// <summary>
        /// Gets or sets the file contents.
        /// </summary>
        public Stream FileContents { get; set; }

        /// <summary>
        /// Gets or sets the Content-Type of the file.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file to download.
        /// </summary>
        public string FileDownloadName { get; set; }
    }
}
