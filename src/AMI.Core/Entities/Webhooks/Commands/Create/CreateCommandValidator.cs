using AMI.Core.Entities.Models;
using FluentValidation;

namespace AMI.Core.Entities.Webhooks.Commands.Create
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    public class CreateCommandValidator : AbstractValidator<CreateWebhookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandValidator"/> class.
        /// </summary>
        public CreateCommandValidator()
        {
            Include(new BaseCommandValidator<WebhookModel>());
            RuleFor(x => x.Secret).NotEmpty();
        }
    }
}
