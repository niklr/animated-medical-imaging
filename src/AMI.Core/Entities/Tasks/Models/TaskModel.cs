using System;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the task.
    /// </summary>
    public class TaskModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the task.
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
        /// Gets or sets the queued date.
        /// </summary>
        public DateTime? QueuedDate { get; set; }

        /// <summary>
        /// Gets or sets the started date.
        /// </summary>
        public DateTime? StartedDate { get; set; }

        /// <summary>
        /// Gets or sets the ended date.
        /// </summary>
        public DateTime? EndedDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the message describing the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the position in queue.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the progress (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the command used to create this task.
        /// </summary>
        public BaseCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the result associated with this task.
        /// </summary>
        public BaseResultModel Result { get; set; }

        /// <summary>
        /// Gets or sets the object associated with this task.
        /// </summary>
        public ObjectModel Object { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static TaskModel Create(TaskEntity entity, IDefaultJsonSerializer serializer)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new TaskModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                QueuedDate = entity.QueuedDate,
                StartedDate = entity.StartedDate,
                EndedDate = entity.EndedDate,
                Status = Enum.TryParse(entity.Status.ToString(), out TaskStatus status) ? status : TaskStatus.Created,
                Message = entity.Message,
                Position = entity.Position,
                Progress = entity.Progress,
                UserId = entity.UserId
            };

            if (Enum.TryParse(entity.CommandType.ToString(), out CommandType commandType))
            {
                switch (commandType)
                {
                    case CommandType.ProcessObjectCommand:
                        model.Command = serializer.Deserialize<ProcessObjectCommand>(entity.CommandSerialized);
                        break;
                    default:
                        break;
                }
            }

            return model;
        }

        /// <summary>
        /// Creates a model based on the given domain entities.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="objectEntity">The domain object entity.</param>
        /// <param name="resultEntity">The domain result entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static TaskModel Create(TaskEntity entity, ObjectEntity objectEntity, ResultEntity resultEntity, IDefaultJsonSerializer serializer)
        {
            var model = Create(entity, serializer);

            if (model == null)
            {
                return null;
            }

            if (objectEntity != null)
            {
                model.Object = ObjectModel.Create(objectEntity);
            }

            if (resultEntity != null)
            {
                model.Result = ResultModel.Create(resultEntity, serializer);
            }

            return model;
        }
    }
}
