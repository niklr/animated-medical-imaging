using FluentValidation;

namespace AMI.Core.Entities.Objects.Commands.Extract
{
    /// <summary>
    /// A validator for extract command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ExtractObjectCommand}" />
    public class ExtractCommandValidator : AbstractValidator<ExtractObjectCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractCommandValidator"/> class.
        /// </summary>
        public ExtractCommandValidator()
        {
            RuleFor(x => x.SourcePath).NotEmpty().MaximumLength(1024);
            RuleFor(x => x.DestinationPath).NotEmpty().MaximumLength(1024);
        }
    }
}
