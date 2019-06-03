using FluentValidation;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{CreateObjectCommand}" />
    public class CreateCommandValidator : AbstractValidator<CreateObjectCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandValidator"/> class.
        /// </summary>
        public CreateCommandValidator()
        {
            RuleFor(x => x.OriginalFilename).NotEmpty().MaximumLength(512);
            RuleFor(x => x.SourcePath).NotEmpty().MaximumLength(1024);
        }
    }
}
