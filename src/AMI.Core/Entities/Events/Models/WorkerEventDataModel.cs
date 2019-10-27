namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the event.
    /// </summary>
    public class WorkerEventDataModel : BaseEventDataModel
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        public BaseWorkerModel Object { get; set; }
    }
}