namespace AMI.Core.Constants
{
    /// <summary>
    /// The names of the background job queues.
    /// </summary>
    public class QueueNames
    {
        /// <summary>
        /// The default background job queue.
        /// </summary>
        public const string Default = "default";

        /// <summary>
        /// The background job queue used for imaging purposes.
        /// </summary>
        public const string Imaging = "imaging";

        /// <summary>
        /// The background job queue used for webhook purposes.
        /// </summary>
        public const string Webhooks = "webhooks";
    }
}
