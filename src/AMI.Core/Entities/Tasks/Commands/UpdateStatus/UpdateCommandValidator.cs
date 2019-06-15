using System;
using FluentValidation;

namespace AMI.Core.Entities.Tasks.Commands.UpdateStatus
{
    /// <summary>
    /// A validator for command requests to update the status of a task.
    /// </summary>
    /// <seealso cref="AbstractValidator{UpdateTaskStatusCommand}" />
    public class UpdateCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandValidator"/> class.
        /// </summary>
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().Must(x =>
            {
                return Guid.TryParse(x, out Guid result);
            });
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
