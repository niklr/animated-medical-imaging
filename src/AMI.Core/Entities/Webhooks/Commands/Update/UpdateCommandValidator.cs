using AMI.Core.Entities.Models;
using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Webhooks.Commands.Update
{
    /// <summary>
    /// A validator for update command requests.
    /// </summary>
    public class UpdateCommandValidator : AbstractValidator<UpdateWebhookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandValidator"/> class.
        /// </summary>
        public UpdateCommandValidator()
        {
            Include(new BaseCommandValidator<WebhookModel>());
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
        }
    }
}
