using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands.ProcessObjectAsync;
using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync
{
    /// <summary>
    /// A validator for process command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ProcessObjectCommand}" />
    public class ProcessCommandValidator : AbstractValidator<ProcessObjectAsyncCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandValidator"/> class.
        /// </summary>
        public ProcessCommandValidator()
        {
            Include(new BaseProcessCommandValidator<TaskModel>());
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
        }
    }
}
