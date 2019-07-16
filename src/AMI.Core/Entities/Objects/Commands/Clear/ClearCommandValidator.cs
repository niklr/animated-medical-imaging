using FluentValidation;

namespace AMI.Core.Entities.Objects.Commands.Clear
{
    /// <summary>
    /// A validator for clear command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ClearObjectsCommand}" />
    public class ClearCommandValidator : AbstractValidator<ClearObjectsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearCommandValidator"/> class.
        /// </summary>
        public ClearCommandValidator()
        {
        }
    }
}
