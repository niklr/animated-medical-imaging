using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.Create
{
    /// <summary>
    /// A command containing information needed to create a task.
    /// </summary>
    public class CreateTaskCommand : IRequest<TaskModel>
    {
        /// <summary>
        /// Gets or sets the command used to create this task.
        /// </summary>
        public BaseCommand Command { get; set; }
    }
}
