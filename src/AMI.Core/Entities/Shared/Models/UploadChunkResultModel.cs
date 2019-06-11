namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing a result of a chunked upload.
    /// </summary>
    public class UploadChunkResultModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether all chunks of a file have been uploaded.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
