using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Tokens.Commands.CreateRefreshToken
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{CreateRefreshTokenCommand}" />
    public class CreateCommandValidator : AbstractValidator<CreateRefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandValidator"/> class.
        /// </summary>
        public CreateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().GuidValidation();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
