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
        }
    }
}
