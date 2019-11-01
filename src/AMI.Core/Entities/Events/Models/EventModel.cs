using System;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about an event that occurred.
    /// An event is created for example when the status of a task changes.
    /// </summary>
    public class EventModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the API version used to render data.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the user identifier that originated the event.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the serialized data associated with the event.
        /// Provided only if the deserialization failed.
        /// </summary>
        public string DataSerialized { get; set; }

        /// <summary>
        /// Gets or sets the data associated with the event.
        /// </summary>
        public BaseEventDataModel Data { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static EventModel Create(EventEntity entity, IDefaultJsonSerializer serializer)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new EventModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                ApiVersion = entity.ApiVersion,
                EventType = Enum.TryParse(entity.EventType.ToString(), out EventType eventType) ? eventType : EventType.Unknown,
                UserId = entity.UserId
            };

            try
            {
                switch (model.EventType)
                {
                    case EventType.TaskCreated:
                    case EventType.TaskUpdated:
                    case EventType.TaskDeleted:
                        model.Data = new TaskEventDataModel()
                        {
                            Object = serializer.Deserialize<TaskModel>(entity.EventSerialized)
                        };
                        break;
                    case EventType.ObjectCreated:
                    case EventType.ObjectUpdated:
                    case EventType.ObjectDeleted:
                        model.Data = new ObjectEventDataModel()
                        {
                            Object = serializer.Deserialize<ObjectModel>(entity.EventSerialized)
                        };
                        break;
                    case EventType.AuditEventCreated:
                        model.Data = new AuditEventDataModel()
                        {
                            Object = serializer.Deserialize<AuditEventModel>(entity.EventSerialized)
                        };
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                model.DataSerialized = entity.EventSerialized;
            }

            return model;
        }
    }
}
