using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Tokens.Commands.UpdateRefreshToken
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    public class UpdateCommandValidator : AbstractValidator<UpdateRefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandValidator"/> class.
        /// </summary>
        public UpdateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().GuidValidation();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
