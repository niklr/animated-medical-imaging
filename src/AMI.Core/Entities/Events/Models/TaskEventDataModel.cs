namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the event.
    /// </summary>
    public class TaskEventDataModel : BaseEventDataModel
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        public TaskModel Object { get; set; }
    }
}
