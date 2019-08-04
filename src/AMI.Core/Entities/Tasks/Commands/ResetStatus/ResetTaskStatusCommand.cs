using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.ResetStatus
{
    /// <summary>
    /// A command containing information needed to reset the status of tasks.
    /// </summary>
    public class ResetTaskStatusCommand : IRequest<bool>
    {
    }
}
