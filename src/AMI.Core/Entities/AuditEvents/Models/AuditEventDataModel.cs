namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing data of an audit event.
    /// </summary>
    public class AuditEventDataModel
    {
        /// <summary>
        /// Gets or sets the audit event entity.
        /// </summary>
        public dynamic Entity { get; set; }

        /// <summary>
        /// Gets or sets the command that caused the audit event.
        /// </summary>
        public dynamic Command { get; set; }
    }
}
