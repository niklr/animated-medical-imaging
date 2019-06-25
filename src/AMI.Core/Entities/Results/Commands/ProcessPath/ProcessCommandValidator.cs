using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands.ProcessObjectAsync;
using FluentValidation;

namespace AMI.Core.Entities.Results.Commands.ProcessPath
{
    /// <summary>
    /// A validator for process command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ProcessPathCommand}" />
    public class ProcessCommandValidator : AbstractValidator<ProcessPathCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandValidator"/> class.
        /// </summary>
        public ProcessCommandValidator()
        {
            Include(new BaseProcessCommandValidator<ProcessResultModel>());
            RuleFor(x => x.SourcePath).NotEmpty().MaximumLength(1024);
            RuleFor(x => x.DestinationPath).NotEmpty().MaximumLength(1024);
        }
    }
}
