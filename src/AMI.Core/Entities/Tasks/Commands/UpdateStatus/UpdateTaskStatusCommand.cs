using AMI.Core.Entities.Models;
using AMI.Domain.Enums;
using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.UpdateStatus
{
    /// <summary>
    /// A command containing information needed to update the status of a task.
    /// </summary>
    /// <seealso cref="IRequest{TaskModel}" />
    public class UpdateTaskStatusCommand : IRequest<TaskModel>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the result.
        /// </summary>
        public string ResultId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
