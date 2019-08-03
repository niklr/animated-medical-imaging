using FluentValidation;

namespace AMI.Core.Entities.Tasks.Commands.ResetStatus
{
    /// <summary>
    /// A validator for command requests to reset the status of a tasks.
    /// </summary>
    public class ResetCommandValidator : AbstractValidator<ResetTaskStatusCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetCommandValidator"/> class.
        /// </summary>
        public ResetCommandValidator()
        {
        }
    }
}
