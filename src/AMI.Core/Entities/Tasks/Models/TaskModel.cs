using System;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the task.
    /// </summary>
    public class TaskModel
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
        /// Gets or sets the identifier of the result.
        /// </summary>
        public string ResultId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the type of the command used to create this task.
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the command used to create this task.
        /// </summary>
        public object Command { get; set; }

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
                Status = Enum.TryParse(entity.Status.ToString(), out TaskStatus status) ? status : TaskStatus.Created,
                Message = entity.Message,
                Position = entity.Position,
                Progress = entity.Progress,
                ObjectId = entity.ObjectId.ToString(),
                ResultId = entity.ResultId.ToString(),
                CommandType = Enum.TryParse(entity.CommandType.ToString(), out CommandType commandType) ? commandType : CommandType.Unknown
            };

            switch (model.CommandType)
            {
                case CommandType.ProcessObjectAsyncCommand:
                    model.Command = serializer.Deserialize<ProcessObjectAsyncCommand>(entity.CommandSerialized);
                    break;
                default:
                    break;
            }

            return model;
        }
    }
}
