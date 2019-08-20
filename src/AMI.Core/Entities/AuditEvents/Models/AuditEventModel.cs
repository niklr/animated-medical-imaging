using System;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums.Auditing;
using XDASv2Net.Model;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing an audit event.
    /// </summary>
    public class AuditEventModel
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the type of the sub event.
        /// </summary>
        public SubEventType SubEventType { get; set; }

        /// <summary>
        /// Gets or sets the XDASv2 event.
        /// </summary>
        public XDASv2Event Xdas { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static AuditEventModel Create(AuditEventEntity entity, IDefaultJsonSerializer serializer)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new AuditEventModel
            {
                Timestamp = entity.Timestamp,
                EventType = Enum.TryParse(entity.EventType.ToString(), out EventType eventType) ? eventType : EventType.INVOKE_SERVICE,
                SubEventType = Enum.TryParse(entity.SubEventType.ToString(), out SubEventType subEventType) ? subEventType : SubEventType.None,
                Xdas = serializer.Deserialize<XDASv2Event>(entity.EventSerialized)
            };

            return model;
        }
    }
}
