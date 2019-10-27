namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the event.
    /// </summary>
    public class AuditEventDataModel : BaseEventDataModel
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        public AuditEventModel Object { get; set; }
    }
}