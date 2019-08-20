using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using XDASv2Net.Model;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing an audit event.
    /// </summary>
    public class AuditEventModel
    {
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
                Xdas = serializer.Deserialize<XDASv2Event>(entity.EventSerialized)
            };

            return model;
        }
    }
}
