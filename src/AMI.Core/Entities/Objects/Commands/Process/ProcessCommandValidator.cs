using FluentValidation;

namespace AMI.Core.Entities.Objects.Commands.Process
{
    /// <summary>
    /// A validator for process command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ProcessObjectCommand}" />
    public class ProcessCommandValidator : AbstractValidator<ProcessObjectCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandValidator"/> class.
        /// </summary>
        public ProcessCommandValidator()
        {
            RuleFor(x => x.SourcePath).NotEmpty().MaximumLength(1024);
            RuleFor(x => x.DestinationPath).NotEmpty().MaximumLength(1024);
        }
    }
}
