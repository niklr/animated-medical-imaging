namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the result of the processing.
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the JSON filename.
        /// </summary>
        public string JsonFilename { get; set; }
    }
}
