using System;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the task to process an object.
    /// </summary>
    /// <seealso cref="TaskModel" />
    public class ProcessObjectTaskModel : TaskModel
    {
        /// <summary>
        /// Gets or sets the command used to create this task.
        /// </summary>
        public ProcessObjectAsyncCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static ProcessObjectTaskModel Create(TaskEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ProcessObjectTaskModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                Status = Enum.TryParse(entity.Status.ToString(), out TaskStatus status) ? status : TaskStatus.Created,
                Position = entity.Position,
                Progress = entity.Progress,
                ObjectId = entity.ObjectId.ToString(),
                ResultId = entity.ResultId.ToString()
            };
        }
    }
}
